export interface ISelectType {
  entity: string
  typeName: string
  id: number
}
export interface IFormCreateNew {
  id?: number
  title: string,
  description: string,
  entityTypeId: number
  groupId: number[]
  piority: number
  status : number
  image?: any
  createDate?: string
  modifiedDate?: string
  canDelete?: boolean
  shortDescription: string
  coverImage?: any
}