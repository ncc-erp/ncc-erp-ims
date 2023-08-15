<template>
  <div class="unlock-timesheet">
      <Row>
          <!----vùng 1---->
           <Col class="col col__item" :xs="24" :sm="24" :md="12" :lg="12">
            <Row>
             <Col :xs="24" :sm="24" :md="24" :lg="24">
             <Card>
                <h4>{{ $t("unlock.fundCurrently") }}</h4>
                 <hr  />
                 <div v-if="isFund" class="amount">
                   <Row>
                     <Col :xs="24" :sm="7" :md="7" :lg="7">
                     <img class="img-ngankho" src="../../../images/ngankho1.png" 
                                style="border-radius:5px"/>
                     </Col>
                     <Col :xs="24" :sm="10" :md="10" :lg="10">
                      <h1 style="font-size:35px;margin-top:20px" >{{ formatPrice(fund) }}</h1>
                     </Col>
                     <Col :xs="24" :sm="7" :md="7" :lg="7">
                     <img class="img-ngankho" src="../../../images/ngankho2.gif" 
                                style="border-radius:5px"/>
                     </Col>
                   </Row>
                
                 </div>
                 <div v-else class="amount" >
                 <span >Chưa có đồng nào 
                   <Icon type="md-sad" size="25" />
                 </span>
                 </div>

             </Card>
             </Col>
             <Col style="margin-top:10px; margin-bottom:10px"  :xs="24" :sm="24" :md="24" :lg="24">
             <Card>
                 <h4>{{ $t("unlock.history") }}</h4>
                 <hr />
                 <Row class="list-history">
                    <Col :xs="24" :sm="9" :md="9" :lg="9">
                      <div><img src="../../../images/history.png" 
                                style="border-radius:10px"/></div>
                    </Col>
                    <Col :xs="24" :sm="14" :md="14" :lg="14" offset="1">
                    <div class="content-history">
                      <ul v-for="(item,index) in listHistorys" :key="index" >
                         <li style="margin-top:5px">
                           <strong>{{item.day}} </strong>
                           <span style="color: indianred">{{item.fullName}}</span> {{item.content}}  
                         </li>
                      </ul>
                    
                     </div></Col>
                  </Row>
             </Card>
             </Col>
            </Row>
           </Col>
           <!----vùng 2---->
           <Col class="col col__item" :xs="24" :sm="24" :md="12" :lg="12">
            <Row>
             <Col :xs="24" :sm="24" :md="24" :lg="24">
             <Card>
                 <h4>{{ $t("unlock.top") }}</h4>
                 <hr />
                <Table
                style="margin-top: 20px"
                class="table-guildines"
                :columns="columnTable"
                :data="listtables"
                border
              >
                <template slot-scope="{ row }" slot="stt">
                   <span>{{row.rank}}</span>
                </template>
                <template slot-scope="{ row }" slot="nguoichoi">
                   <span>{{row.fullName}}</span>
                </template>
                <template slot-scope="{ row }" slot="kimnguyenbao">
                  <span>{{formatPrice(row.amount)}}</span>
                </template>           
              </Table>
    
             </Card>
             </Col>
             <Col style="margin-top:10px"  :xs="24" :sm="24" :md="24" :lg="24">
             <Card>
                 <h4>{{ $t("unlock.guide") }}</h4>
                 <hr />
                 <div class="notify">
                Tổng thời gian log timesheet và thời gian xin nghỉ trong 1 ngày phải bằng 8.
                 Nếu 1 tuần có tồn tại 1 bản ghi bất thường, chúng tôi sẽ báo cho bạn trước. 
                 Bạn có thể kiểm tra lại để log lại thời gian cho phù hợp,
                 rồi mới quyết định quay về đây để chấp nhận mức phạt và unlock Timesheet.
                 </div>
                 <hr />
                 <div class="notify" style="color:indianred; cursor: pointer;}">
                   <span @click="onpenpopUp"><u><strong>TimeSheet của bạn có đang bị khóa ? Xem chi tiết tại đây</strong></u></span>
                 </div>
             </Card>
             </Col>
            </Row>
           </Col> 
      </Row>
      <Modal
        v-model="isShowPopup"
        :footer-hide=true
        :title="title"
        
       ><PopupUnLock  @onSubmit="onpenpopUp" v-if="isShowPopup"/>
    </Modal>
  </div>
</template>
<script lang="ts">
import moment from 'moment';
import PopupUnLock from "./components/popup-unlock.vue"
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
@Component({ 
  name: 'unlock-timesheet',
  components: {
      PopupUnLock
  },
   filters: {
    moment(value: any) {
      return moment(value).format("DD-MM-YYYY");
    },
  },
})
export default class UnlockTimesheet extends Vue {
private columnTable = [];
private listHistorys = [];
private listtables = [];
private fund : number = 0;
private isFund = false;
private isShowPopup = false;
title = 'Unlock Timesheet'
async created() {
     this.columnTable = [
      {
        title: this.$t("unlock.stt"),
        slot: "stt",
        align: "center",
        width: 130,
      },
      {
        title: this.$t("unlock.player"),
        slot: "nguoichoi",
      },
      {
        title: this.$t("unlock.kimnguyen"),
        slot: "kimnguyenbao",
        align: "center",
      },
    ];
    this.fetchData();
}
async fetchData() {
   let respone  = await this.$store.dispatch({
      type: "unlockTimeSheet/topUserUnlock",
   });
   this.listtables = respone.result.listUnlock
   if(!respone.result.totalAmount){
    this.isFund =false;
   }else { 
     this.isFund = true; 
     this.fund = respone.result.totalAmount
   }
   let respone1  = await this.$store.dispatch({
      type: "unlockTimeSheet/getAllHistory",
   });
   this.listHistorys = respone1.result;
}
onpenpopUp() {
    this.isShowPopup = !this.isShowPopup
    this.fetchData();
}
 formatPrice(value) {
        let val = (value/1).toFixed(0).replace('.', ',')
        console.log(val)
        return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".")
    }

}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
h4 {
    margin-bottom: 10px;
}
.amount {
    color:indianred;
    text-align: center;
    margin-bottom: 20px;
    margin-top: 20px;
}
.list-history {
    margin-top: 15px;
}
.notify {
    margin-top: 10px;
    margin-bottom: 10px;
}
.content-history{
  height: 42vh;
  overflow: auto;
  list-style-type: none;
}
.img-ngankho{
  max-height:90px ;
}
@media only screen and (max-width: 768px) {
  img{
    margin-bottom: 20px;
   // max-height: 200px;
  }
 }
</style>

