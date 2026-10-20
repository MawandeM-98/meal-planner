import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DishCount {
  name: string;
  count: number;
}

export interface MealPlanResult {
  totalFed: number;
  dishes: DishCount[];
}

@Injectable({ providedIn: 'root' })
export class MealsService {
  private http = inject(HttpClient);
  private url = 'http://localhost:5080/api/meals';

  getBestPlan(): Observable<MealPlanResult> {
    return this.http.get<MealPlanResult>(this.url);
  }
}