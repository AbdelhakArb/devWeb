export interface ListeDeNaissance {
  listeDeNaissanceId?: number;
  compteParentId: number;
  nomListeDeNaissance: string;
  dateCreationListe?: Date;
  datePrevuPourAccouchement?: Date;
  statusListe?: string;
  lieuListe?: string;
}