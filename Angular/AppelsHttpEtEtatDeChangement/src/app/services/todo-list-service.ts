import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { TodosResponse } from '../models/Todos';

@Injectable({
  providedIn: 'root',
})

export class TodoListService {

  private apiUrl = 'https://dummyjson.com/todos';
  private http = inject(HttpClient);

  getTodos():Observable<TodosResponse> {
    return this.http.get<TodosResponse>(this.apiUrl);

  } 
}
