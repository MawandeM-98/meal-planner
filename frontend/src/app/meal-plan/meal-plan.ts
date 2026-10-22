import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { MealsService, MealPlanResult } from '../meals.service';

@Component({
  selector: 'app-meal-plan',
  templateUrl: './meal-plan.html',
  styleUrl: './meal-plan.css',
})
export class MealPlan implements OnInit {
  private meals = inject(MealsService);

  plan = signal<MealPlanResult | null>(null);
  error = signal(false);

  // One group of plates per dish, biggest first. Every plate gets a running
  // number so the page-load animation can set them down one after another.
  groups = computed(() => {
    const dishes = [...(this.plan()?.dishes ?? [])].sort((a, b) => b.fed - a.fed);
    let next = 0;
    return dishes.map((dish) => {
      const seats = Array.from({ length: dish.fed }, () => next++);
      return { ...dish, seats };
    });
  });

  pantry = computed(() => {
    const plan = this.plan();
    if (!plan) return [];

    return Object.keys(plan.used).map((name) => {
      const used = plan.used[name];
      const left = plan.leftovers[name];
      const total = used + left;
      return {
        name,
        used,
        left,
        total,
        pips: Array.from({ length: total }, (_, i) => i < used),
      };
    });
  });

  dishesCooked = computed(() =>
    (this.plan()?.dishes ?? []).reduce((sum, dish) => sum + dish.count, 0),
  );

  unitsUsed = computed(() => this.pantry().reduce((sum, row) => sum + row.used, 0));
  unitsTotal = computed(() => this.pantry().reduce((sum, row) => sum + row.total, 0));

  leftoverText = computed(() => {
    const left = this.pantry()
      .filter((row) => row.left > 0)
      .map((row) => `${row.left} ${row.name.toLowerCase()}`);
    return left.length ? `Left over: ${left.join(', ')}.` : 'Nothing left over.';
  });

  ngOnInit() {
    this.load();
  }

  load() {
    this.error.set(false);
    this.meals.getBestPlan().subscribe({
      next: (result) => this.plan.set(result),
      error: () => this.error.set(true),
    });
  }
}