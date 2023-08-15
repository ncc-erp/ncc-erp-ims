<template>
  <div class="dashboard">
    <Row class="wrapper-dashbroad">
      <Col
        class="col col__item col-offset quick-links-container left-db"
        :xs="10"
        :sm="10"
        :md="7"
        :lg="6"
      >
        <Row>
          <LayoutLeft></LayoutLeft>
        </Row>
      </Col>
    
      <Col
        class="col col__item wrap-content"
        :xs="10"
        :sm="10"
        :md="9"
        :lg="13"
      >
        <Card class="news-card format" style="border: none">
          <Row class="list-post">
            <div v-for="(item, index) in newsList" :key="index">
              <div class="lastest-news">
                <Row type="flex" justify="space-between">
                  <Col
                    class="col col__mb col__item m-content news-item"
                    span="24"
                  >
                    <ListItem
                      :data.sync="newsList[index]"
                      v-on:changeIsShow="listenChange"
                    />
                  </Col>
                </Row>
              </div>
            </div>
          </Row>
          <Row class="list-post fake">
            <div v-for="(item, index) in newsListFake" :key="index">
              <div class="lastest-news">
                <Row type="flex" justify="space-between">
                  <Col
                    class="col col__mb col__item m-content news-item"
                    span="24"
                  >
                    <ListItem
                      :data.sync="newsListFake[index]"
                      v-on:changeIsShow="listenChange"
                    />
                  </Col>
                </Row>
              </div>
            </div>
          </Row>
        </Card>
      </Col>
       <Col
        class="col col__item col-offset quick-links-container right-db"
        :xs="10"
        :sm="10"
        :md="8"
        :lg="10"
      >
        <Row>
          <LayoutRight></LayoutRight>
        </Row>
      </Col>
    </Row>
    <!----->
    <Modal v-model="isShowTask" :footer-hide="true" width="70%">
      <popupTask v-if="isShowTask" />
    </Modal>
  </div>
</template>
<script lang="ts">
import {
  Component,
  Vue,
  Inject,
  Emit,
  Prop,
  Watch,
} from "vue-property-decorator";
import Calendar from "../../components/fullCalendar/calendar.vue";
import ListItem from "../../components/list-item.vue";
import { InforEntity } from "../../store/entities/inforEntity";
import { EventBus } from "../../main";
import URL from "../../lib/url";
import SessionStorageInfoPage from "../../types/session-storage/session-storage";
import BannerCarousel from "../../components/banner-carousel.vue";
import moment from "moment";
import popupTask from "../task/components/popupTask.vue";
import DashboardDto from "@/store/entities/dashboard";
import LayoutLeft from "../../components/layout-left.vue";
import LayoutRight from "../../components/layout-right.vue"

@Component({
  components: {
    Calendar,
    ListItem,
    BannerCarousel,
    popupTask,
    LayoutLeft,
    LayoutRight
  },
  filters: {
    moment(value: any) {
      return moment(value).format("YYYY-MM-DD HH:mm");
    },
  },
})
export default class Dashboard extends Vue {
  private fdd = true;
  private InforEntity = InforEntity;
  private currentPage: number = 1;
  private listFace = [{ text: "hell0" }];
  private pageSize: number = 12;
  private searchQuery: string = "";
  private totalCount: number = 1;
  private newsList = new Array<DashboardDto>();
  private newsListFake = new Array<DashboardDto>();
  private newsListfillter: any = [];
  private isApprove: boolean;
  private isShowTask = false;

  created() {
    SessionStorageInfoPage.clear("New");
    SessionStorageInfoPage.clear("Event");
    SessionStorageInfoPage.clear("Policy");
    SessionStorageInfoPage.clear("GuildLine");
    this.fetchData();
  }
  listenChange(item) {
    this.newsList.forEach((i, index) => {
      if (i.id === item.id) {
        i = item;
      }
    });
  }
  mounted() {
    EventBus.$on("inputData", (payLoad) => {
      this.searchQuery = payLoad;
      this.getListPost(this.searchQuery);
    });
  }
  onChangePage(page) {
    this.currentPage = page;
    this.fetchData();
  }

  async getListPost(searchQuery: string) {
    await this.$store
      .dispatch({
        type: "dashboard/getAll",
        data: this.searchQuery,
      })
      .then((res) => {
        this.newsList = res.data.result.map((item) => {
          item.isImage = item.image == null ? false : true;
          item.image = URL + item.image;
          item.coverImage = URL + item.coverImage;
          item.isShow = false;
          return item;
        });
      });
  }

  async fetchData() {
    this.getListPost(null);
    // let respone = await this.$store.dispatch({
    //   type: "task/viewJobInDashboard",
    // });
    // if (respone.length !== 0) {
    //   this.isShowTask = true;
    // }
  }
  viewDetail(item: any) {
    switch (item.entityName) {
      case InforEntity.New:
        this.$router.push({
          name: "NewsDetail",
          params: { newsId: item.id, commentId: null },
        });
        this.$router.go(0);
        break;
      case InforEntity.Event:
        this.$router.push({
          name: "EventDetail",
          params: { eventId: item.id, commentId: null },
        });
        break;
      case InforEntity.Policy:
        this.$router.push({
          name: "policyDetail",
          params: { policyId: item.id, commentId: null },
        });
        break;
      case InforEntity.GuideLine:
        this.$router.push({
          name: "GuideDetail",
          params: { guideId: item.id, commentId: null },
        });
        break;
      default:
        this.$router.push({
          name: "NewsDetail",
          params: { newsId: item.id, commentId: null },
        });
    }
  }
  // openDashboardDetail(id: number) {
  //   this.newsList.forEach((element) => {
  //     if (id === element.id) {
  //       this.viewDetail(element);
  //     }
  //   });
  // }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
@font-face {
  font-family: "San Francisco";
  src: url("https://applesocial.s3.amazonaws.com/assets/styles/fonts/sanfrancisco/sanfranciscodisplay-regular-webfont.woff");
}
* {
  color: #333;
}
.single-page-con {
    overflow: hidden;
    background-color: #f9f9f9;
  }
.dashboard {
  // overflow-y: scroll;
  margin-bottom: 20px;
  // height: 90vh;
  // overflow: auto;
  .wrap-content{
    width: calc(100% - 67rem);;
  }
  .left-db {
    padding: 0;
    background: $bg-color-dashboard 0% 0% no-repeat padding-box;
    max-width: 33rem;
    width: 33rem;
    // height: 100%;
    // overflow-y: auto;
    // overflow-x: hidden;
    ::-webkit-scrollbar {
      display: none;
    }
  }
  .right-db{
    background: $bg-color-dashboard 0% 0% no-repeat padding-box;
    max-width: 33rem;
    width: 33rem;
    // width: 20%;
    // height: 88vh;
    // overflow: auto;
    // overflow-x: hidden;
    ::-webkit-scrollbar {
      display: none;
    }
    // float: right;
  }
  .list-post {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 2.4rem;
  }
  .fake {
    margin-top: 55px;
    margin-bottom: 22px;
  }

  .ivu-col-span-lg-18 {
    width: calc(100% - 350px);
  }
  .ivu-row:not(.wrapper-dashbroad.ivu-row) {
    .left-db {
      overflow: hidden;
    }
    .right-db{
      overflow: hidden;
    }
  }
  .ivu-row:before,
  .ivu-row:after {
    display: none;
  }
  .wrap-content {
    background-color: $bg-color-dashboard;
    // padding: 0 2% 0 0;
    // float: right;
    // overflow-y: scroll;
    // height: 100%;
  }
  span {
    // font-family: San Francisco;
    font-weight: 600;
  }
  .user-calendar {
    border: none;
  }
  .news-item {
    padding-left: 0;
    padding-right: 0;
  }
  .wrapper-dashbroad {
    // overflow: auto;
    height: 90vh;
  }
  .img-face {
    max-width: 300px;
    height: 77px;
    width: 77px;
    object-fit: cover;
  }
  .image-face {
    width: 90px;
    display: flex;
    justify-content: center;
  }
  .option {
    padding-left: 16px;
  }
  .item {
    margin-bottom: 15px;
  }
  .banner {
    margin-bottom: 30px;
  }
  .more {
    font-size: 16px;
    cursor: pointer;
    font-weight: 700;
    width: fit-content;
  }
  .icon-new {
    height: 60px;
    padding-bottom: 6px;
  }
  .title-left,
  .related_news {
    text-align: left;
    font-size: 23px;
    margin-bottom: 10px;
    font-weight: bold;
    text-transform: uppercase;
    color: #000000;
  }
  .accepted {
    font-size: 15px;
    color: #858585;
    display: flex;
    align-items: center;
    img {
      filter: grayscale(1);
      padding-right: 5px;
      padding-bottom: 4px;
    }
  }
  .title-left {
    position: relative;
    padding-bottom: 12px;
    font-family: Segoe UI;
  }
  .title-left-treasury {
    position: relative;
    padding-bottom: 8px;
    font-family: Segoe UI;
    text-align: left;
    font-size: 23px;
    font-weight: bold;
    text-transform: uppercase;
    color: #000000;
  }
  .quick-links-card::before {
    height: 1px;
    display: block;
    margin-top: 3px;
    width: 100%;
    background: #ccc;
    content: "";
    position: absolute;
    left: 0;
    bottom: 5px;
  }
  .my-face-card {
    box-shadow: none;
    border: none;
  }
  ::v-deep .ivu-card-body {
    padding: 0px;
  }
  .format {
    padding-left: 13px;
    padding-top: 18px;
    padding-bottom: 18px;
    background-color: $bg-color-dashboard;
  }
  // .title-left::before {
  //   height: 1px;
  //   display: block;
  //   margin-top: 3px;
  //   width: 100%;
  //   background: #ccc;
  //   content: '';
  //   position: absolute;
  //   left: 0;
  //   bottom: 0;
  // }
  .related_news::after {
    height: 1px;
    display: block;
    margin-top: 3px;
    width: 97%;
    background: #ccc;
    content: "";
  }
  .title-right {
    text-align: right;
  }
  p {
    // margin-bottom: 10px;
    text-transform: uppercase;
    font-size: 18px;
    font-weight: bold;
    padding: 5px 0;
  }
  .lastest-news {
    margin-bottom: 45px;
    //padding-top: 10px;
  }
  .user-calendar {
    padding: 10px;
  }
  .quick-links li {
    margin-bottom: 10px;
    border-radius: 2px;
    position: relative;
  }
  //   .quick-links li .tooltip-social {
  //   visibility: hidden;
  //   background-color: #fff;
  //   color: #000;
  //   text-align: center;
  //   padding: 3px 5px;
  //   max-width: 125px;
  //   border: 1px solid black;
  //   font-size: 13px;
  //   white-space: nowrap;
  //   /* Position the tooltip */
  //   position: absolute;
  //   left: -22px;
  //   z-index: 1;
  // }
  // .quick-links li:hover .tooltip-social {
  //   visibility: visible;
  // }
  .quick-links li a {
    display: block;
    width: 100%;
  }
  //   .quick-links li a img:hover {
  //     animation: shake 0.8s;
  //   }
  //   @keyframes shake {
  //     0% { transform: translate(1px, 1px) rotate(0deg); }
  //     10% { transform: translate(-1px, -2px) rotate(-1deg); }
  //     20% { transform: translate(-3px, 0px) rotate(1deg); }
  //     30% { transform: translate(3px, 2px) rotate(0deg); }
  //     40% { transform: translate(1px, -1px) rotate(1deg); }
  //     50% { transform: translate(-1px, 2px) rotate(-1deg); }
  //     60% { transform: translate(-3px, 1px) rotate(0deg); }
  //     70% { transform: translate(3px, 1px) rotate(-1deg); }
  //     80% { transform: translate(-1px, -1px) rotate(1deg); }
  //     90% { transform: translate(1px, 2px) rotate(0deg); }
  //     100% { transform: translate(1px, -2px) rotate(-1deg); }
  //  }
  .quick-links-card {
    //  margin-bottom: 10px;
    border: none;
  }
  .treasury-box {
    padding-bottom: 18px;
    .treasury-title {
      font-size: 19px;
      font-weight: 700;
    }
    .treasury {
      font-size: 35px;
      color: #b24747;
      font-weight: bold;
      font-family: system-ui;
    }
  }
  .unlock-timesheet {
    width: 316px;
    padding-bottom: 5px;
    cursor: pointer;
  }
  .btn-option {
    border: none;
    background-color: #f3f3f3;
    padding: 5px 0px 6px;
    width: 65px;
  }
  .accept {
    font-size: 15px;
    color: green;
    display: flex;
    align-items: center;
    img {
      padding-right: 5px;
    }
  }
  .reject {
    font-size: 15px;
    margin-top: 2px;
    color: red;
    display: flex;
    align-items: center;
    img {
      padding-right: 5px;
    }
  }
  .treasury-box {
    padding-bottom: 18px;
    .treasury-title {
      font-size: 19px;
      font-weight: 700;
    }
    .treasury {
      font-size: 35px;
      color: #b24747;
      font-weight: bold;
      font-family: system-ui;
    }
  }
  .unlock-timesheet {
    width: 316px;
    padding-bottom: 5px;
    cursor: pointer;
  }
  .btn-option {
    border: none;
    background-color: #f3f3f3;
    padding: 5px 0px 6px;
    width: 65px;
  }
  .accept {
    font-size: 15px;
    color: green;
    display: flex;
    align-items: center;
    img {
      padding-right: 5px;
    }
  }
  .reject {
    font-size: 15px;
    margin-top: 2px;
    color: red;
    display: flex;
    align-items: center;
    img {
      padding-right: 5px;
    }
  }
  .quick-links-card:hover {
    box-shadow: none;
  }
  .news-card:hover {
    box-shadow: none;
  }
  .calendar-card {
    border: none;
  }
  .calendar-card:hover {
    box-shadow: none;
  }
}

 @media only screen and (min-width: 1421px) and (max-width: 1517px) {
  .dashboard {
    .list-post {
      grid-template-columns: repeat(2, minmax(0, 1fr)) !important;
    }

  }
}
@media only  screen and (min-width: 1281px)  and (max-width: 1420px) {
  .dashboard {
    .list-item {
      .box {
        height: 13vw;
      }
    }
    .list-post {
      grid-template-columns: repeat(2,1fr);
    }

}
}

@media only  screen and (min-width: 1179px)  and (max-width: 1280px) {
  .dashboard {
    .list-item {
      .box {
        height: 13vw;
      }
    }
    .list-post {
      grid-template-columns: repeat(2,1fr);
    }

}
}

@media only screen and (min-width: 921px) and (max-width: 1180px) {
  .dashboard {
    .list-post {
      grid-template-columns: repeat(1, minmax(0, 1fr));
    }
    }
  }
 
@media only screen and (min-width: 769px) and (max-width: 920px) {
  .dashboard {
    .list-post {
      grid-template-columns: repeat(1, 32rem);
      justify-content: center;
    }
    
  }
}
@media only screen and (max-width: 768px) {
  .dashboard {
    .left-db {
      position: relative;
      width: 100% !important;
      height: auto;
      // bật tắt left-db
      display: none;
    }
  }
  .dashboard {
    .list-post {
      grid-template-columns: repeat(2, minmax(0, 1fr));
      grid-row-gap: 0px;
      grid-column-gap: 2% !important;
    }

    .item {
      display: block;
      text-align: center;
      margin-bottom: 15px;
    }
    
  }
}
@media only screen and (max-width: 518px) {
  .dashboard {
    // height: 100%;
    .list-post {
      grid-template-columns: repeat(1, minmax(0, 38rem));
      grid-row-gap: 2.4rem;
    }

  }
  .main .single-page-con{
    overflow: hidden;
    background-color: #f9f9f9;
}
 
}
@media only screen and (max-width: 425px) {
  .related_news::after {
    width: 100% !important;
  }
  .right-db{
    max-width: 100% !important;
  }
}
</style>
