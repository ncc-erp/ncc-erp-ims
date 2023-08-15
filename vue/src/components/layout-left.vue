<template>
  <Col
    class="col col__item col-offset quick-links-container left-db"
    :xs="24"
    :sm="24"
    :md="24"
    :lg="24"
  >
    <!-- vùng NCC8 -->
    <!-- <Card class="quick-links-card format">
      <h3 class="title-left">Radio NCC8</h3>
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24" class="pt-10">
          <video id="video" controls></video>
        </Col>
      </Row>
    </Card> -->

    <!--vùng link-->
    <Card class="quick-links-card format">
      <h3 class="title-left">Quick Links</h3>
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24">
          <ul class="quick-links">
            <!-- <li>
              <b
                ><a
                  href="https://komu.nccsoft.vn/"
                  target="_blank"
                  class="text-quick-links"
                  ><img class="icon-new" src="../images/Komu.svg" />
                  <span>Komu</span></a
                ></b
              >
            </li> -->
            <li>
              <b
                ><a
                  href="http://timesheet.nccsoft.vn/"
                  target="_blank"
                  class="text-quick-links"
                  ><img class="icon-new" src="../images/Timesheet.svg" />
                  <span>Timesheet</span></a
                ></b
              >
            </li>
            <li>
              <b
                ><a
                  href="https://ops.nccsoft.vn/"
                  target="_blank"
                  class="text-quick-links"
                  ><img class="icon-new" src="../images/devOps.svg" />
                  <span>DevOps</span></a
                ></b
              >
            </li>
            <li>
              <b
                ><a
                  href="http://talent.nccsoft.vn/"
                  target="_blank"
                  class="text-quick-links"
                  ><img
                    class="icon-new"
                    src="../images/Talent-management.svg"
                  />
                  <span>Talent</span></a
                ></b
              >
              <!-- <span class="tooltip-social">Talent management</span> -->
            </li>
            <li>
              <b
                ><a
                  href="http://project.nccsoft.vn/"
                  target="_blank"
                  class="text-quick-links"
                  ><img class="icon-new" src="../images/Komu.svg" />
                  <span>Project</span></a
                ></b
              >
              <!-- <span class="tooltip-social">Talent management</span> -->
            </li>
          </ul>
        </Col>
      </Row>
    </Card>

    <!-- Vùng bài ghim -->

    <!-- Vùng ngân khố -->
    <Card class="quick-links-card format">
      <div class="meta-left tooltip" style="opacity: 1">
          <h3 class="title-left tooltip">Tổng quỹ phạt</h3>
          <span class="tooltiptext">Tổng quỹ phạt hiện tại của toàn công ty <br>
            chưa giải ngân</span>
      </div>
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24" class="wrapper-treasury">
          <div
            class="treasury-box d-flex justify-content-center align-items-center"
          >
            <img src="/img/icons/money-bag.png" class="icon-money-bag" />
            <h2 class="treasury" @click="openDialogFunAmountHistories" style="cursor: pointer">
              {{ treasuryMoney }}
              <img src="/img/icons/dollar.png" class="icon-dollar" />
            </h2>
          </div>
        </Col>
      </Row>
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24">
          <img
            class="unlock-timesheet-format"
            @click="onpenpopUp"
            style="cursor: pointer"
            src="../images/Unlock-timesheet.svg"
          />
        </Col>
      </Row>
    </Card>

    <Row class="quick-links-card format ">
      <Col :xs="24" :sm="24" :md="24" :lg="24" class="wrapper-treasury">
        <div class="row-check-in" @click="$router.push('/extension/checkin')">
          <Button class="btn-check-in">
            <span class="text-check-in title-left"
              ><img
                class="img-check-in"
                src="../images/Checkpoint.png"
                alt=""
              />
              Check In</span
            ></Button
          >
        </div>
      </Col>
    </Row>

    <!-- Face Id-->
    <Card class="my-face-card format">
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24">
          <p class="title-left">Faces</p>
          <Row type="flex" justify="space-between" class="mt-10 f-dir-col">
            <hooper group="group1" :autoPlay = "true" :playSpeed=4000 :transition=2000>
              <slide v-for="(item, index) in this.listFace" :key="index" >
                <div data-toggle="tooltip" :title="item.projectInfo" class=" mb-15">
                  <div class="image-face">
                    <img class="img-face" :src="item.img" alt="img" />
                  </div>
                  
                  <div class="option user-info" >
                    <p class="userName"> {{ item.fullName }}</p>
                    <p><span class="userBranch">{{item.branch}}</span><span v-if="item.branch && item.type"> - </span>{{item.type}}</p>
                    <p class="text-time-verify">
                      {{ item.checkInAt }}
                    </p>
                  </div>
                </div>
              </slide>
            </hooper>
          </Row>
        </Col>
      </Row>
    </Card>

    <Modal v-model="isShowPopup" :footer-hide="true" :title="title"
      ><PopupUnLock @onSubmit="onpenpopUp" v-if="isShowPopup" />
    </Modal>
    <Modal>
      <FundAmountHistory v-if="isShowDialogFundAmountHistory" :isShow="isShowDialogFundAmountHistory"/>
    </Modal>


  </Col>
</template>
<script lang="ts">
import { Component, Vue, Watch } from "vue-property-decorator";
import moment from "moment";
import {
  Hooper,
  Slide,
  Navigation as HooperNavigation,
  Pagination as HooperPagination,
} from "hooper";
import Hls from "hls.js";
import FundAmountHistory from "./fund-amount-history.vue";
@Component({
  components: {},
  filters: {
    moment(value: any) {
      return moment(value).format("YYYY-MM-DD HH:mm");
    },
  },
})
@Component({
  components: {
    Hooper,
    Slide,
    HooperNavigation,
    HooperPagination,
    FundAmountHistory
  },
})
export default class LayoutLeft extends Vue {
  private listFace = [];
  private listHistorys = [];
  private pageSize: number = 10;
  private pageNumber: number = 1;
  private treasuryMoney: any = 0;
  private isShowMore: boolean = false;
  private isFund = false;
  private fund: number = 0;
  private checkInMoney: any = 0;
  private isShowPopup = false;
  private title: string = "Unlock Timesheet";
  private params = new URLSearchParams(window.location.search);
  public isShowDialogFundAmountHistory:boolean = false;
  created() {
    this.viewFace();
    this.getTreasuryMoney();
  }

  async getTreasuryMoney() {
    let respone = await this.$store.dispatch({
      type: "unlockTimeSheet/getTreasuryMoney",
    });
    this.treasuryMoney = new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(respone.toString());
    this.checkInMoney = new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(0);
  }
  async loadMore() {
    this.isShowMore = !this.isShowMore;
  }
  async appRovedorReject(isApprove, imageId) {
    let data = {
      imageId: imageId,
      isApprove: isApprove,
    };
    await this.$store.dispatch({
      type: "dashboard/decideImage",
      data: data,
    });
    this.viewFace();
  }
  async viewFace() {
    let response;
    response = await this.$store.dispatch({
      type: "dashboard/getListImage",
    });
    this.listFace = response.map((el) => {
      el.checkInAt = moment(String(el.checkInAt)).format(
        "DD-MM-YYYY HH:mm:ss"
      );
      return el;
    });
    this.listFace.forEach(face =>{
      face.projectInfo = `${face.projectUsers.length > 0 ? '- Dự án: ' : ''}\n \t`
      face.projectUsers.forEach(p =>{
        face.projectInfo+=`+ ${p.projectName} ${p.pms.length > 0 ? '('+[...p.pms].join(",")+')' : ''}
          `
      })
    })
  }
  onpenpopUp() {
    this.isShowDialogFundAmountHistory = false;
    this.isShowPopup = !this.isShowPopup;
  }
  openDialogFunAmountHistories(){
    this.isShowDialogFundAmountHistory = !this.isShowDialogFundAmountHistory;
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
* {
  color: #333;
}
//Thêm meme vào đây :V
::v-deep .ivu-modal-wrap {
  /* backdrop-filter: url("https://i.gifer.com/7Uw.gif") right top no-repeat; */

  //bỏ comment đây và thay hình
  /* background-image: url("https://i.gifer.com/7Uw.gif"); */
  /* filter: blur(8px);
  -webkit-filter: blur(8px); */
  /* height: 100%; */
  /* background-position: center; */
  /* background-repeat: no-repeat; */
  /* background-size: cover; */
}
@keyframes fadeIn {
  0% { opacity: 0; }
  100% { opacity: 1; }
}
.projectUser{
  display: none;
  height: fit-content;

}
.showMore{
  display: block;
  height: fit-content;
}
.d-flex {
  position: relative;
}
.tooltip-wrap .tooltip-content {
  display: none;
  position: absolute;
  top: -5%;
  left: 5%;
  right: 5%;
  background-color: #fff;
  padding: .5em;
  width: 200px;
  box-shadow: 0 0 1.2rem rgba(0, 0, 0, 0.2);
  z-index: 1000;
  margin: 10px;
}
.d-flex:hover .tooltip-content {
  display: block;
}
.d-flex:hover .f-dir-col{
  height: 220px !important;
}
.dashboard {
  .row-check-in {
    border-radius: 10px;
    box-shadow: 0 0 1.2rem rgba(0, 0, 0, 0.2);
    background-color: $color-white-primary;
    display: flex;
    justify-content: center;
    padding: 1.2rem 0;

    .btn-check-in {
      border: none;

      .img-check-in {
        height: 4.2rem;
        margin-right: 1.2rem;
      }
    }
  }
  .text-quick-links span {
    color: $color-gray-light-8;
  }
  span {
    font-weight: 600;
  }
  .user-calendar {
    border: none;
  }
  .news-item {
    padding-left: 0;
  }
  .img-face {
    max-width: 300px;
    height: 205px;
    width: 225px;
    object-fit: cover;
    margin-bottom: 15px;
    animation: fadeIn 5s;
  }
  .image-face {
    width: auto;
    display: flex;
    justify-content: center;
  }
  .item {
    margin-bottom: 12px;
  }
  .banner {
    margin-bottom: 30px;
  }
  .more {
    padding-top: 2px;
    font-size: 16px;
    cursor: pointer;
    font-weight: 700;
    width: fit-content;
    color: #000000;
    opacity: 0.7;
  }
  .angle-up {
    transform: rotate(180deg);
  }
  .icon-new {
    height: 4.8rem;
    padding-bottom: 6px;
  }
  .more {
    div {
      display: flex;
      align-items: center;
      gap: 5px;
    }
  }
  .more-icon {
    height: 31px !important;
    width: 31px !important;
    margin-bottom: 10px !important;
    padding: 5px 7px 3px;
    background: #d6d7d8;
    border-radius: 50%;
  }
  .title-left,
  .related_news {
    text-align: center;
    font-size: 2rem;
    margin-bottom: 2.2rem;
    font-weight: bold;
    text-transform: uppercase;
    color: $color-gray-medium;
    letter-spacing: 0.8px;
  }
  .accepted {
    font-size: 15px;
    color: #858585;
    display: flex;
    align-items: center;
    img {
      filter: grayscale(1);
      padding-right: 6px;
      padding-bottom: 2px;
    }
  }
  .title-left {
    position: relative;
    font-family: Segoe UI;
  }
  .quick-links-card {
    border-radius: 0;
  }
  .my-face-card {
    box-shadow: none;
    border: none;
  }
  .format {
    padding: 2rem 15px 0px 30px;
    background-color: #f3f3f3;

    &:first-child {
      padding-top: 1.8rem;
    }
  }
  .unlock-timesheet-format {
    width: 100%;
    padding: 15px 0;
  }
  .bg-color {
    background-color: #f3f3f3;
  }
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
  .mb-10{
    margin-bottom: 30px;
  }
  .user-info{
    cursor: pointer;
    overflow: auto;
    font-size: 14px;
    font-weight: 600;
    .userName{ 
      color: #b24747;
    }
    .userBranch , .userType{
      color: #6786a1
    }

  }
  .lastest-news {
    margin-bottom: 45px;
  }
  .user-calendar {
    padding: 10px;
  }
  .quick-links {
    justify-content: space-between;
    align-items: end;
    width: 100%;
  }
  .quick-links li {
    margin-bottom: 17px;
    border-radius: 2px;
    position: relative;
    text-align: center;
  }
  .quick-links li a {
    display: flex;
    flex-direction: column;
    width: 100%;
    text-decoration: none;
  }
  .quick-links-card {
    border: none;
  }
  .treasury-box {
    .treasury-title {
      font-family: "San Francisco";
      font-size: 18px;
      line-height: 21px;
      font-weight: 600;
    }
    .treasury {
      font-size: 30px;
      line-height: 36px;
      color: #b24747;
      font-family: system-ui;
      text-align: center;
      margin-left: 1rem;
    }
  }
  .unlock-timesheet {
    width: 100%;
    cursor: pointer;
  }
  .btn-option {
    border: none;
    background: none;
    padding: 11px 0px 0px;
    width: 65px;
  }
  .accept {
    font-size: 15px;
    color: #2dd256;
    display: flex;
    align-items: center;
    img {
      padding-right: 6px;
    }
  }
  .reject {
    font-size: 15px;
    margin-top: 2px;
    color: red;
    display: flex;
    align-items: center;
    img {
      padding-right: 6px;
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
.mt-10 {
  margin-top: 10px;
}
.mb-15 {
  margin-bottom: 15px;
}
.pt-10 {
  padding-top: 10px;
}
.f-dir-col {
  flex-direction: column;
}
.text-time-verify {
  font-size: 12px;
  color: #00000078;
  white-space: nowrap;
}
.mr-10 {
  margin-right: 10px;
}
.mr-25 {
  margin-right: 25px;
}
.hooper {
  height: 100% !important;
  animation: fadeIn 5s;
}
li.hooper-slide {
  display: flex;
  justify-content: center;
}
li.hooper-slide.is-current {
  .img-face-slide {
    border: 2px solid tomato;
  }
}
li [aria-hidden = true]{
  display: none;
}
@media (max-width: 1440px) {
  .more {
    font-size: 14px !important;
    img {
      width: 22px;
      height: 22px;
    }
  }
}
@media (max-width: 768px) {
  .dashboard {
    .item {
      display: flex;
      text-align: center;
      margin-bottom: 12px;
    }
    .image-face {
      width: fit-content;
    }
    .box-faceid {
      display: flex;
    }
    .quick-links {
      width: 95%;
    }
    .icon-new {
      height: 5vw;
      width: 5vw;
    }
    .text-time-verify {
      font-size: 12px;
    }
    .btn-option {
      font-size: 12px;
    }
  }
}
@media only screen and (max-width: 517px) {
  .wrap-content {
    padding: 0 !important;
  }
  .dashboard {
    .icon-new {
      height: 15vw;
      width: 15vw;
    }
  }
}
@media only screen and (max-width: 415px) {
  .dashboard .option {
    padding-left: 10px !important;
    .text-time-verify {
      font-size: 14px;
    }
    .box-faceid {
      display: flex !important;
      span {
        font-size: 12px;
      }
      img {
        width: 16px;
      }
    }
  }
}
.tooltip {
  position: relative;
  z-index: unset;
}

.tooltip .tooltiptext {
  font-size: 13px;
  visibility: hidden;
  width: auto;
  background: #fff;
  text-align: center;
  padding: 3px 0;
  position: absolute;
  top: 33%;
  left: 0%;
  white-space: nowrap;
  border: 1px solid #000;
  padding: 4px;
  z-index: 99;
}

.tooltip:hover .tooltiptext {
  visibility: visible;
}
</style>
