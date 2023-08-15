export default interface IGettingCalendar {
  toDate?: string
  fromDate?: string
}

export default interface ICalendar {
  date: string,
  entity: string
  entityId: number
  id: number
  note: string
  url: string
  userId: number
  formattedDate?: string
  title?: string
}