import { IClueModel } from "./IClueModel";

export interface IGameModel {
  id: string;
  tournamentId: string;
  created: Date;
  clues: IClueModel[];
}
