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
            <li>
              <b
                ><a
                  href="https://komu.nccsoft.vn/"
                  target="_blank"
                  class="text-quick-links"
                  ><img class="icon-new" src="../images/Komu.svg" />
                  <span>Komu</span></a
                ></b
              >
            </li>
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
          </ul>
        </Col>
      </Row>
    </Card>
    <!-- Vùng ngân khố -->
    <Card class="quick-links-card format">
      <h3 class="title-left-treasury">Ngân Khố</h3>
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24" class="wrapper-treasury">
          <div class="treasury-box">
            <b class="treasury-title">Unlock timesheet</b>
            <h2 class="treasury">{{ treasuryMoney }}</h2>
          </div>
          <div class="treasury-box">
            <b class="treasury-title">Check-in</b>
            <h2 class="treasury">{{ checkInMoney }}</h2>
          </div>
        </Col>
      </Row>
    </Card>
    <Card class="quick-links-card unlock-timesheet pb-17">
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24" class="bg-color">
          <img
            class="unlock-timesheet-format"
            @click="onpenpopUp"
            style="cursor: pointer"
            src="../images/Unlock-timesheet.svg"
          />
        </Col>
      </Row>
    </Card>
    <Card class="my-face-card format">
      <Row>
        <Col :xs="24" :sm="24" :md="24" :lg="24">
          <p class="title-left">My face</p>

          <Row type="flex" justify="space-between" class="mt-10 f-dir-col">
            <Col
              :xs="24"
              :sm="8"
              :md="8"
              :lg="6"
              v-for="(item, index) in this.isShowMore
                ? listFace
                : listFace.slice(0, 5)"
              :key="index"
              class="item d-flex align-items-center"
            >
              <div class="image-face">
                <img class="img-face" :src="item.image" alt="img" />
              </div>
              <div class="option">
                <p class="text-time-verify">
                  {{ item.timeVerify }}
                </p>
                <div class="d-flex box-faceid">
                  <Button
                    class="btn-option mr-25"
                    v-if="item.isApprove === false"
                    @click="appRovedorReject(true, item.imageVerifyId)"
                    ><span class="accept"
                      ><img src="../images/Accept.svg" alt="" /> Accept</span
                    ></Button
                  >
                  <Button
                    class="btn-option mr-10"
                    :disabled="true"
                    v-if="item.isApprove === true"
                    @click="appRovedorReject(true, item.imageVerifyId)"
                    ><span v-bind:class="{ accepted: item.isApprove }"
                      ><img src="../images/Accept.svg" alt="" /> Accepted</span
                    ></Button
                  >
                  <Button
                    class="btn-option"
                    v-if="item.isApprove === false"
                    @click="appRovedorReject(false, item.imageVerifyId)"
                    ><span class="reject"
                      ><img src="../images/Reject.svg" alt="" /> Reject</span
                    ></Button
                  >
                </div>
              </div>
            </Col>
            <div class="more" v-if="listFace.length > 5" @click="loadMore()">
              <div v-if="!this.isShowMore">
                <img class="more-icon" src="../images/Point-down.svg" alt="" />
                <span style="margin-bottom: 10px; font-size: 16px !important">Xem thêm</span>
              </div>
              <div v-else>
                <img
                  class="angle-up more-icon"
                  src="../images/Point-down.svg"
                  alt=""
                />
                <span style="margin-bottom: 10px; font-size: 16px !important">Thu gọn</span>
              </div>
            </div>
            <div class="more" v-else-if="!listFace">No data!</div>
          </Row>
        </Col>
      </Row>
    </Card>
    <Modal v-model="isShowPopup" :footer-hide="true" :title="title"
      ><PopupUnLock @onSubmit="onpenpopUp" v-if="isShowPopup" />
    </Modal>
  </Col>
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
import moment from "moment";
import PopupUnLock from "../views/extension/unlock-timesheet/components/popup-unlock.vue";
import Hls from "hls.js";
@Component({
  components: {},
  filters: {
    moment(value: any) {
      return moment(value).format("YYYY-MM-DD HH:mm");
    },
  },
})
@Component({
  name: "unlock-timesheet",
  components: {
    PopupUnLock,
  },
  filters: {
    moment(value: any) {
      return moment(value).format("DD-MM-YYYY");
    },
  },
})
export default class LayoutLeftPost extends Vue {
  private listFace = [];
  private listHistorys = [];
  private pageSize: number = 10;
  private treasuryMoney: any = 0;
  private isShowMore: boolean = false;
  private isFund = false;
  private fund: number = 0;
  private checkInMoney: any = 0;
  private isShowPopup = false;
  private params = new URLSearchParams(window.location.search);

  created() {
    this.viewFace();
    this.getTreasuryMoney();
    // this.radioNcc8();
  }
  // radioNcc8() {
  //   var url = "http://172.16.100.153:8080/live/ncc8.m3u8";
  //   if (Hls.isSupported()) {
  //     var video = document.getElementById("video") as HTMLMediaElement;
  //     var hls = new Hls();
  //     hls.attachMedia(video);
  //     hls.loadSource(url);
  //     hls.on(Hls.Events.MEDIA_ATTACHED, function() {
  //       video.play();
  //       hls.on(Hls.Events.MANIFEST_PARSED, function(event, data) {
  //         console.log("run success");
  //       });
  //     });
  //   }
  // }
  async getTreasuryMoney() {
    let respone = await this.$store.dispatch({
      type: "unlockTimeSheet/topUserUnlock",
    });
    this.treasuryMoney = new Intl.NumberFormat("vi-VN", {
      style: "currency",
      currency: "VND",
    }).format(respone.result.totalAmount);
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
      data: {
        pageNumber: 1,
        pageSize: this.pageSize,
      },
    });
    this.listFace = response.items.map((el) => {
      el.isApprove = el.clockStatus === "APPROVED" ? true : false;
      el.timeVerify = moment(String(el.timeVerify)).format(
        "DD-MM-YYYY HH:mm:ss"
      );
      return el;
    });
  }
  onpenpopUp() {
    this.isShowPopup = !this.isShowPopup;
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
* {
  color: #333;
}
.dashboard {
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
    height: 3vw;
    width: 3vw;
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
    text-align: left;
    font-size: 20px;
    margin-bottom: 22px;
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
      padding-right: 6px;
      padding-bottom: 2px;
    }
  }
  .title-left {
    position: relative;
    font-family: Segoe UI;
  }
  .title-left-treasury {
    position: relative;
    padding-bottom: 18px;
    font-family: Segoe UI;
    text-align: left;
    font-size: 20px;
    line-height: 27px;
    font-weight: bold;
    text-transform: uppercase;
    color: #000000;
  }
  .quick-links-card {
    border-bottom: 0.5px solid #cacaca !important;
    border-radius: 0;
  }
  .my-face-card {
    box-shadow: none;
    border: none;
  }
  .format {
    padding: 18px 0px 0px 30px;
    background-color: #f3f3f3;
  }
  .unlock-timesheet-format {
    width: 100%;
    padding: 20px 25px 15px 30px;
    background-color: #f3f3f3;
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
  p {
    text-transform: uppercase;
    font-size: 15px;
    font-weight: 600;
    line-height: 20px;
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
  .wrapper-treasury {
    .treasury-box:first-child {
      padding-bottom: 18px;
    }
  }
  .treasury-box {
    padding-bottom: 15px;
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
      font-weight: bold;
      font-family: system-ui;
    }
  }
  .unlock-timesheet {
    width: 100%;
    cursor: pointer;
  }
  .btn-option {
    border: none;
    background-color: #f3f3f3;
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
  font-size: 15px;
  color: #00000078;
  white-space: nowrap;
}
.mr-10 {
  margin-right: 10px;
}
.mr-25 {
  margin-right: 25px;
}
@media (max-width: 1440px) {
  .img-face {
    height: 60px !important;
    width: 60px !important;
  }
  .text-time-verify {
    font-size: 13px !important;
  }
  .more {
    font-size: 14px !important;
    img {
      width: 22px;
      height: 22px;
    }
  }
  .btn-option {
    width: 55px !important;
    margin-right: 0px;
    padding-top: 0 !important;
    span {
      font-size: 10px !important;
    }
    img {
      width: 17px;
    }
  }
}
@media only screen and (max-width: 1050px) {
  .dashboard {
    
    .unlock-timesheet-format {
      padding: 20px 10px 15px 20px;
    }
    .img-face {
      width: 60px;
      height: 60px;
    }
    .btn-option {
      padding-top: 3px;
    }
    .text-time-verify {
      font-size: 12px !important;
    }
    .option {
      padding-left: 10px;
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
@media (max-width: 900px) {
  .dashboard .icon-new {
    height: 4vw;
    width: 4vw;
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
</style>
