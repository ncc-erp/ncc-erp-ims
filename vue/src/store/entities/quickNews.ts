import Entity from "./entity";

export default class QuickNewsDto extends Entity<any> {
    content: string;
    hover:string;
    creationTime: Date;
    isExpanded: false;
    desHeight: number;
  }

