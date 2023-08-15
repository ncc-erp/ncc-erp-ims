import Entity from './entity'
export default class News extends Entity<number>{
    title: string
    typeName: string
    pageNumber: number
    pageSize: number
}
