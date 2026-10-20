import { Component, OnInit, inject, signal } from '@angular/core';
import { MealsService, MealPlanResult } from '../meals.service';

@Component({
  selector: 'app-meal-plan',
  templateUrl: './meal-plan.html',
  styleUrl: './meal-plan.css',
})
export class MealPlan implements OnInit {
  private meals = inject(MealsService);

  plan = signal<MealPlanResult | null>(null);
  error = signal('');

  ngOnInit() {
    this.meals.getBestPlan().subscribe({
      next: (result) => this.plan.set(result),
      error: () => this.error.set('Could not reach the API. Is the backend running?'),
    });
  }
}