import Entity from "./entity";
export default class DashboardDto extends Entity<any> {
  entityName: string;
  title: string;
  description: string;
  entityTypeId: string;
  typeName: string;
  groupId: any[];
  piority: number;
  status: number;
  effectiveStartTime: Date;
  effectiveEndTime: Date;
  createDate: Date;
  modifiedDate: Date;
  image: string;
  CoverImage: string;
  shortDescription: string;
  isShow: boolean;
  totalComment: any;
  userLike: any;
}