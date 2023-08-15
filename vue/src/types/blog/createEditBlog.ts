import { install } from "vuex";

export interface IFormCreateBlog {
    id?: number;
    title: string;
    detail: string;
  }

export interface IFormUpdateInforBlog {
  userId: number;
  description: string;
  backgroundImages: any;
  musicBackground: string;
  nickName: string;
  avatar: any;
}
export interface IObjectFile {
  path: string;
  file: File;
  buffer: any;
}

export interface IFormSearch {
  hashtag: string;
}