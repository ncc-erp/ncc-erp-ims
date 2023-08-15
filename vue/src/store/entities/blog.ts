import Entity from './entity'
export default class Blog extends Entity<number>{
    title: string
    typeName: string
    pageNumber: number
    pageSize: number
}