import { Article } from './article'; 

export interface PanierReservationDto {
  articles: Article[]; // Doit correspondre à la liste dans ton Backend
}