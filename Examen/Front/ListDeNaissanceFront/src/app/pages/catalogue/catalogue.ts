import { Component, OnInit, inject, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Article } from '../../services/api/models/article';
import { ArticleService } from '../../services/article';

@Component({
  selector: 'app-catalogue',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './catalogue.html',
  styleUrls: ['./catalogue.css']
})
export class CatalogueComponent implements OnInit {
  readonly articleService = inject(ArticleService);
  readonly articleAjoute = output<Article>();

  ngOnInit(): void {
    this.articleService.loadArticles();
  }

  onAjouter(article: Article): void {
    this.articleAjoute.emit(article);
  }
}