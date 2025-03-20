
export enum MatchTabViewType {
  MatchView = "MatchView",
  AddScoreView = "AddScoreView"
}

//Persistance stored item type
export type StoredItem<T>  = {
  value: T;
  expiry: Date | undefined | null;
}


export const DefaultExpiryMintures = 120;
export const DefaultApplicationPrefix = "TP"