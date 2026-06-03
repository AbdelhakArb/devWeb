import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {TodosResponse} from '../../models/Todos';
import { TodoListService } from '../../services/todo-list-service';

@Component({
  selector: 'app-todo-list-component',
  imports: [FormsModule],
  templateUrl: './todo-list-component.html',
  styleUrl: './todo-list-component.css',
})
export class TodoListComponent {
  isLoading = false;
  todos = signal<TodosResponse | null>(null);

  constructor(private todoListService: TodoListService) {
    this.todoListService.getTodos().subscribe({
      next: (response: TodosResponse) => {
        console.log('Todos fetched successfully:', response);
        this.todos.set(response);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching todos:', error);
        this.isLoading = false;
      }
    });
  }

}
