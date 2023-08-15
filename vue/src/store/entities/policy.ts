export default interface IPolicy {
  title: string,
  description: string,
  entityTypeId: number,
  groupId: number,
  piority: number,
  status: number,
  effectiveStartTime: any,
  effectiveEndTime: any,
  image: any,
}

export enum EStatus {
  DRAFT = 1000,
  PENDING = 2000,
  RETURN = 3000,
  APPROVED = 4000,
  HIDDEN = 5000,
  DISABLE = 6000
}

export enum EAction {
  CREATE = 'CREATE',
  SUBMIT = 'SUBMIT',
  APPROVE = 'APPROVE',
  RETURN = 'RETURN',
  HIDDEN = 'HIDDEN',
  DISABLE = 'DISABLE'
}