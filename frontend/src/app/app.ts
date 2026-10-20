import { Component } from '@angular/core';
import { MealPlan } from './meal-plan/meal-plan';

@Component({
  selector: 'app-root',
  imports: [MealPlan],
  template: '<app-meal-plan />',
})
export class App {}