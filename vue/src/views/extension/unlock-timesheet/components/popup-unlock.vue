<template>
  <div class="popup-unlock">
    <Row>
      <Col v-if="isPM" class="col col__item content" :xs="24" :sm="24" :md="24" :lg="24">
          <b><p class="pm" style="color: orange;">Bạn là PM:</p></b>
          <div v-if="isUnlockToApprove == false && totalLockedPM > 0">
            <span>Bạn có <b class="pm">{{totalLockedPM}}</b> tuần chưa approve/reject cho thành viên trong dự án.</span>
          </div>
          <div v-if="isUnlockToApprove == false && totalLockedPM == 0">
            <p>Timesheet trong project của bạn hoạt động bình thường</p>
            <span>Bạn có muốn unlock để reject/approve timesheet không?</span>
          </div>
          <div v-if="isUnlockToApprove == true">
            <span >Timesheet của bạn đã được <span class="unlocked">Unlock for PM</span></span>
          </div>
      </Col>
       <Col class="col col__item content" :xs="24" :sm="24" :md="24" :lg="24">
          <b><p class="pm">Bạn là nhân viên:</p></b>
          <p>(<i>Sau khi unlock bạn có thể Update TimeSheet từ : <b style="color: blue;">{{firstDateCanLogIfUnlock}}</b> đến now</i>)</p>
          <div v-if="isUnlockToLog == false && hasListDate != false">
            <span>Hiện tại chúng tôi thấy bạn có </span>
            <span><strong>{{totalWeek}}</strong></span>
            <span> tuần có lỗi sai <Icon type="md-sad" size="20" />. Cụ thể như sau: </span>
          </div>
          <div v-if="isUnlockToLog == true">
            <span >Timesheet của bạn đã được <span class="unlocked">Unlock for Employee</span></span>
          </div>
          <div v-if="hasListDate == false && isUnlockToLog == false">
              Timesheet của bạn hoạt động bình thường.<br>
              Bạn có muốn unlock để sửa TimeSheet không?
          </div>
      </Col>
      <Col v-if="isUnlockToLog == false && listDatas != null && hasListDate != false " class="col col__item" :xs="24" :sm="24" :md="24" :lg="24">
         <Table         
                :columns="columnTable"
                :data="listDatas"
                border>
                <template slot-scope="{ row }" slot="week">
                   <span>{{row.startDate}} - {{row.endDate}}</span>
                </template>
                <template slot-scope="{ row }" slot="day">
                  <span>
                    <ul style="list-style-type: none;margin-top:5px; margin-bottom:5px"
                        v-for="(item, index) in row.day" :key="index">
                      <li>{{item}}</li>
                    </ul>
                  </span>
                </template>           
              </Table>
   
      </Col>
      <Col style="text-align: center" :xs="24" :sm="24" :md="24" :lg="24">
      <Button v-if="hasListDate == false && isUnlockToLog == false" size="large" class="btn btn--create"  @click="openModalConfirmSaturday" >
          <p>Unlock for Employee </p>
          <p> ( 20,000 )</p>
        </Button>
        <Button v-if="hasListDate && isUnlockToLog == false" size="large" class="btn btn--create" style="margin-right: 20px;"  @click="openModalConfirmLog" >
          <p>Unlock for Employee </p>
          <p> ( {{formatPrice(totalMount)}} )</p>
        </Button>
        <Button v-if="isUnlockToApprove == false && isPM" size="large" style="background-color: orange !important;margin-left: 20px;" class="btn btn--create"  @click="openModalConfirmApprove" >
          <p>Unlock for PM </p>
          <p v-if="totalLockedPM > 0"> ( {{formatPrice(totalMountPM)}} )</p>
          <p v-if="totalLockedPM == 0"> ( 50,000 )</p>
        </Button> 
      </Col>
    </Row>
    <Modal
        v-model="isShowConfirmUnlockLog"
        :title="$t('common.notification')"  
     >
     <p>{{ $t("unlock.notify unlock to log timesheet") }}</p>
     <Row slot="footer" class="button-zone">
     <Button
        class="button"
        size="default"
        @click="agreeUnlockToLog()"
      >{{ $t("common.accept") }}</Button>
      <Button
        class="button"
         type="error"
         size="default"
         ghost
         @click="isShowConfirmUnlockLog = false"
      >{{ $t("common.cancel") }}</Button>
      </Row>
    </Modal>
    <Modal
        v-model="isShowConfirmUnlockApprove"
        :title="$t('common.notification')"
     >
     <p>{{ $t("unlock.notify unlock to approve timesheet") }}</p>
     <Row slot="footer" class="button-zone">
     <Button
        class="button"
        size="default"
        @click="agreeUnlockToApprove()"
      >{{ $t("common.accept") }}</Button>
      <Button
        class="button"
         type="error"
         size="default"
         ghost
         @click="isShowConfirmUnlockApprove = false"
      >{{ $t("common.cancel") }}</Button>
      </Row>
    </Modal>
    <Modal
        v-model="isShowConfirmUnlockSaturday"
        :title="$t('common.notification')"  
     >
     <p>{{ $t("unlock.notify unlock to log timesheet") }}</p>
     <Row slot="footer" class="button-zone">
     <Button
        class="button"
        size="default"
        @click="agreeUnlockSaturday()"
      >{{ $t("common.accept") }}</Button>
      <Button
        class="button"
         type="error"
         size="default"
         ghost
         @click="isShowConfirmUnlockSaturday = false"
      >{{ $t("common.cancel") }}</Button>
      </Row>
    </Modal>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
@Component({ 
  name: 'PopupUnLock',
  components: {
  },
})
export default class PopupUnLock extends Vue {
private isClose = false;
private isShowConfirmUnlockLog = false;
private isShowConfirmUnlockApprove = false;
private isShowConfirmUnlockSaturday = false;
private isUnlockToLog = null;
private isUnlockToApprove = null;
private hasListDate = false;
private listDatas = [];
private columnTable = [];
private totalWeek: number = 0;
private totalMount: number = 0;
private totalMountPM: number = 0;
private totalLockedPM : number = 0;
private isPM : false;
private firstDateCanLogIfUnlock : number = 0;
private
@Emit('onSubmit')
   private onSubmit() {
    return
   }  
  close(index) {
    this.listDatas[index].isClose =  !this.listDatas[index].isClose
  }
  async created(){
     this.columnTable = [
      {
        title: this.$t("unlock.week"),
        slot: "week",
        align: "center",
      },
      {
        title: this.$t("unlock.day"),
        slot: "day",
        align: "center",
      },
    ];
    this.fetchData();
   
  }
  async fetchData() {
     let respone = await this.$store.dispatch({
      type: "unlockTimeSheet/getAllTimesheetLocked",
       });
    // if ( respone.result.amount == 0) {
    //   this.isUnlock = true;
    // }
    // else {
    //   this.isUnlock = false;
    //   if(respone.result.lockedEmployee.length == 0) {
    //     this.hasListDate = false;
    //   }else {
    //     this.hasListDate = true;
    //   }
    //   this.listDatas = respone.result.lockedEmployee;
    //   console.log(respone.result)
    //   this.totalMount = respone.result.amount;
    //   this.totalWeek = respone.result.lockedEmployee.length;
    //   this.totalLockedPM = respone.result.lockedPM;
    // }
    
    this.isUnlockToLog = respone.result.isUnlockLog;
    this.isUnlockToApprove = respone.result.isUnlockApprove;
    this.isPM = respone.result.isPM;
    if(respone.result.lockedEmployee == null)
      this.hasListDate = false;
    else{
      if(respone.result.lockedEmployee.length == 0) {
      this.hasListDate = false;
      }else {
        this.hasListDate = true;
      }
      this.totalWeek = respone.result.lockedEmployee.length;
    }    
    this.listDatas = respone.result.lockedEmployee;
    this.totalMount = respone.result.amount;
    this.totalMountPM = respone.result.amountPM;
    this.totalLockedPM = respone.result.lockedPM;
    this.firstDateCanLogIfUnlock = respone.result.firstDateCanLogIfUnlock;
  }
  openModalConfirmLog() {
    this.isShowConfirmUnlockLog = true;
  }
  openModalConfirmApprove() {
    this.isShowConfirmUnlockApprove = true;
  }
  openModalConfirmSaturday() {
    this.isShowConfirmUnlockSaturday= true;
  }
  async  agreeUnlockToLog() {
    await this.$store.dispatch({
      type: "unlockTimeSheet/unlockToLogTimesheet",
    });
    this.$Message.info(this.$t('unlock.unlockSuccess'));
    this.onSubmit();
  }
  async  agreeUnlockToApprove() {
    console.log("begin")
    await this.$store.dispatch({
      type: "unlockTimeSheet/unlockToApproveTimesheet",
    });
    this.$Message.info(this.$t('unlock.unlockSuccess'));
    this.onSubmit();
  }
  async agreeUnlockSaturday() {
    await this.$store.dispatch({
      type: "unlockTimeSheet/unlockTimesheetSaturday",
    });
    this.$Message.info(this.$t('unlock.unlockSuccess'));
    this.onSubmit();
  }
  formatPrice(value) {
        let val = (value/1).toFixed(0).replace('.', ',')
        return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
  }
   
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.list-content{
overflow: auto;
border: 1px solid #e8eaec;
margin-top: 5px;
height: 60px;
} 
Button {
margin-top: 10px;
cursor: pointer;
}
.content {
    margin-bottom: 15px;
    font-size: 14px;
}
.not-unlock{
  font-size: 17px;
   color: indianred;
}
.unlocked{
  font-size: 17px;
   color: #087cce;
}
.pm {
   color: indianred;
}
.btn {
  background: indianred !important;
  color: #fff !important;
  border: none;
}

</style>
<style>

</style>

