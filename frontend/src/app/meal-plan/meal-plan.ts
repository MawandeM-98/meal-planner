import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { MealsService, MealPlanResult } from '../meals.service';

@Component({
  selector: 'app-meal-plan',
  templateUrl: './meal-plan.html',
  styleUrl: './meal-plan.css',
})
export class MealPlan implements OnInit {
  private meals = inject(MealsService);

  // Photo files in frontend/public/images, by dish name.
  private photos: Record<string, string> = {
    Pizza: 'pizza.jfif',
    Pasta: 'pasta.jpg',
    Sandwich: 'sandwhich.jfif',
  };

  plan = signal<MealPlanResult | null>(null);
  error = signal(false);

  // Dishes that feed the most people come first.
  dishes = computed(() => [...(this.plan()?.dishes ?? [])].sort((a, b) => b.fed - a.fed));

  // One entry per person fed, so each plate can be coloured by its dish.
  seats = computed(() =>
    this.dishes().flatMap((dish) => Array.from({ length: dish.fed }, () => dish.name)),
  );

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

  // Dishes without a listed photo fall back to <dish name>.jpg.
  photo(dish: string) {
    const file = this.photos[dish] ?? `${dish.toLowerCase()}.jpg`;
    return `/images/${file}`;
  }
}