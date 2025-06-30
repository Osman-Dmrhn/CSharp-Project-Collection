import { Category } from "./category";
import { Author } from "./Author";

export class Blog {
  id?: number;
  baslik?: string;
  icerik?: string;
  resimPath?: string;
  kategoriler?: Category[];
  yazar?: Author;
  }