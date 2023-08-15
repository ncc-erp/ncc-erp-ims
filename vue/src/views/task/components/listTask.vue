<template>
  <div class="task">
    <Row>
      <Col class="col col__item" :xs="24" :sm="24" :md="12" :lg="12">
        <span style="font-size: 22px"><strong>{{ $t("task.alljob") }}</strong></span>
        <Button class="button-create" @click="onCreateNews" ghost 
          >{{ $t("common.create") }}</Button
        >
      </Col>
    </Row>
    <!-----Vùng giao việc----->
    <Row >
      <Col class="col col__item" :xs="24" :sm="24" :md="12" :lg="12">
        <Row>
          <Col :xs="24" :sm="24" :md="24" :lg="24">
            <Card style="margin-bottom:5px">
              <h4>{{ $t("task.assign") }}
                 <Icon class="point_icon" type="md-arrow-dropdown" v-if="!isAssign" @click="hideCreatedJob()" size="25" />
                 <Icon class="point_icon" type="md-arrow-dropup" v-else @click="hideCreatedJob()" size="25" />
                 <Icon class="point_icon" type="ios-refresh" style="float:right" @click="refeshCreated()" size="27" />
              </h4>
              <span></span>
              <hr />
              <Row style="margin-top: 20px; font-size: 13px" v-if="!isAssign">
                <Col class="col col__item form_search" :xs="24" :sm="3" :md="3" :lg="3"
                  ><span>{{ $t("common.selectStatus") }}</span></Col
                >
                <Col class="col col__item" :xs="24" :sm="8" :md="8" :lg="8">
                  <Select
                    v-model="createdJobSearch.status"
                    style="width: 100%"
                    :placeholder="$t('common.selectStatus')"
                    @on-change="getCreatedJob()"
                  >
                    <Option value="" :placeholder="$t('common.all')">{{
                      $t("common.all")
                    }}</Option>
                    <Option
                      v-for="item in statusList"
                      :value="item.id"
                      :key="item.id"
                      >{{ item.name }}</Option
                    >
                  </Select>
                </Col>
                <!----->
                <Col  class="col col__item form_search" :xs="24" :sm="3" :md="3" :lg="3"
                  ><span>{{ $t("task.receiver") }}</span></Col
                >
                <Col class="col col__item" :xs="24" :sm="9" :md="9" :lg="9">
                  <Select
                    v-model="createdJobSearch.receiverId"
                    @on-change="getCreatedJob()"
                    style="width: 100%"
                    filterable
                    clearable
                    :placeholder="$t('task.receiver')"
                  >
                    <Option :value="0" :placeholder="$t('common.all')">{{
                      $t("common.all")
                    }}</Option>
                    <Option
                      v-for="item in listUsers"
                      :value="item.id"
                      :key="item.id"
                      >{{ item.name }}</Option
                    >
                  </Select>
                </Col></Row
              >
              <Row v-if="!isAssign"
                style="margin-top: 20px; font-size: 13px; margin-bottom: 20px"
              >
                <Col class="col col__item form_search" :xs="24" :sm="3" :md="3" :lg="3"
                  ><span>{{ $t("task.deadline") }}</span></Col 
                >
                <Col class="col col__item" :xs="24" :sm="8" :md="8" :lg="8">
                  <DatePicker
                    v-model="createdJobSearch.startDate"
                    @on-change="getCreatedJob()"
                    type="date"
                    :placeholder="$t('task.startTime')"
                    style="width: 100%"
                  ></DatePicker>
                </Col>
                <Col
                  span="3"
                  style="
                    margin-top: 10px;
                    text-align: right;
                    margin-bottom: 10px;
                  "
                  ><span></span
                ></Col>
                <Col class="col col__item" :xs="24" :sm="9" :md="9" :lg="9">
                  <DatePicker
                   @on-change="getCreatedJob()"
                    v-model="createdJobSearch.endDate"
                    type="date"
                    :placeholder="$t('task.endTime')"
                    style="width: 100%"
                  ></DatePicker>
                <span class="valid-date" v-if="validDateCreate">Ngày đến phải lớn hơn ngày bắt đầu !</span>
                </Col>
              </Row>
              <!----->
              <hr />
              <Table v-if="!isAssign"
                style="margin-top: 20px"
                class="table-guildines"
                :columns="columnTable"
                :data="listCreatedJob"
              >
                <template slot-scope="{ row }" slot="name" >
                  <span 
                    v-if="row.status == 0"
                    @click="viewTaskcanEdit(row)"
                    class="name"
                    >{{ row.name }}</span
                  >
                  <span style="word-wrap:break-word;"
                    v-else
                    class="nameAfterDone"
                    @click="viewTaskcanEdit(row)"
                    >{{ row.name }}</span
                  >
                </template>
                <template slot-scope="{ row }" slot="deadline">
                  <span v-if="row.deadline === null">No deadline</span>
                  <span v-else>
                    <span>{{ row.deadline | moment }}</span
                    ><br />
                    <span v-if="!row.inTimelimit" style="color: red"
                      >(quá hạn)</span
                    >
                  </span>
                </template>
                <template slot-scope="{ row }" slot="status">
                  <Select v-model="row.status" @on-change="changeStatus(row)">
                    <Option
                      v-for="(item, index) of statusList"
                      :key="index"
                      :value="item.id"
                      >{{ item.name }}</Option
                    >
                  </Select>
                </template>
                <template slot-scope="{ row }" slot="receiver">
                  <ul style="list-style-type: none;" v-for="item in row.users" :key="item.userId">
                    <li>{{item.fullName}}</li>
                  </ul>
                  <!-- <span>{{ row.reporter }}</span> -->
                </template>
                <template slot-scope="{ row }" slot="action">
                  <Icon
                    @click="confirmDelete(row)"
                    type="md-close-circle"
                    size="25"
                    class="point_icon"
                  />
                  <Modal
                    v-model="isShowConfirmDelete"
                    :title="$t('common.notification')" 
                    :data="row"    
                  >
                    <p>{{ $t("task.confirmDeleteNotice") }}</p>
                    <Row slot="footer" class="button-zone">
                      <Button
                        class="button btn-add"
                        size="default"
                        @click="deleteTask()"
                        >{{ $t("common.accept") }}</Button
                      >
                      <Button
                        type="error"
                        size="default"
                        ghost
                        @click="isShowConfirmDelete = false"
                        >{{ $t("common.cancel") }}</Button
                      >
                    </Row>
                  </Modal>
                  <!-- <p class="detail" @click="viewTaskcanEdit(row)"><u>Chi tiết</u></p>
                  <p class="delete" @click="deleteTask(row.id)"><u>Xóa</u></p>  -->
                </template>
              </Table>
            </Card></Col
          >
          <Col v-if="!isAssign" style="margin-top:15px; margin-bottom:20px" :xs="24" :sm="24" :md="24" :lg="24" >
          <Card>
            <Row>
              <Col :xs="24" :sm="7" :md="7" :lg="7" class="col col__item form_quick">
                <Select
                  v-model="formQuickAdd.userId" multiple
                  style="width: 100%"
                  filterable
                  clearable
                  :placeholder="$t('task.receiver')"
                >
                  <Option
                    v-for="item in listUsers"
                    :value="item.id"
                    :key="item.id"
                    >{{ item.name }}</Option
                  >
                </Select>
              </Col>
              <Col :xs="24" :sm="16" :md="16" :lg="16" class="col col__item form_quick">
                <Input
                  v-model="formQuickAdd.name"
                  :rows="2"
                  style="width: 100%"
                  :placeholder="$t('task.contentForAdd')"
                  type="textarea"
              /></Col>
              <Col :xs="24" :sm="24" :md="1" :lg="1" class="col col__item form_quick">
                <Button
                  class="button-create"
                  @click="quickAdd()"
                  ghost          
                  > <Icon
                    type="md-add"
                    size="15"
                  /></Button
                >
              </Col>
            </Row></Card></Col>
        </Row>
      </Col>
<!-------Vùng việc của tôi ----->
      <Col class="col col__item" :xs="24" :sm="24" :md="12" :lg="12">
        <Row>
          <Col :xs="24" :sm="24" :md="24" :lg="24">
            <Card>
              <h4>{{ $t("task.myjob") }}
                <Icon class="point_icon" type="md-arrow-dropdown" v-if="!isRecive" @click="hideMyJob()" size="25" /> 
                <Icon class="point_icon" type="md-arrow-dropup" v-else @click="hideMyJob()" size="25" />
                <Icon class="point_icon" type="ios-refresh" style="float:right" @click="refeshMyJob()" size="27" />
              </h4>
              <hr />
              <Row v-if="!isRecive" style="margin-top: 20px; font-size: 13px">
                <Col class="col col__item form_search" :xs="24" :sm="3" :md="3" :lg="3"
                  ><span>{{ $t("common.selectStatus") }}</span></Col
                >
                <Col class="col col__item" :xs="24" :sm="8" :md="8" :lg="8">
                  <Select
                    v-model="MyJobSearch.status"
                    style="width: 100%"
                    :placeholder="$t('common.selectStatus')"
                    @on-change="getMyJob()"
                  >
                    <Option value="" :placeholder="$t('common.all')">{{
                      $t("common.all")
                    }}</Option>
                    <Option
                      v-for="item in statusList"
                      :value="item.id"
                      :key="item.id"
                      >{{ item.name }}</Option
                    >
                  </Select>
                </Col>
                <!----->
                <Col class="col col__item form_search" :xs="24" :sm="3" :md="3" :lg="3"
                  ><span>{{ $t("task.assignee") }}</span></Col
                >
                <Col class="col col__item" :xs="24" :sm="9" :md="9" :lg="9">
                  <Select
                    v-model="MyJobSearch.reporterId"
                    @on-change="getMyJob()"
                    style="width: 100%"
                    filterable
                    clearable
                    :placeholder="$t('task.assignee')"
                  >
                    <Option :value="0" :placeholder="$t('common.all')">{{
                      $t("common.all")
                    }}</Option>
                    <Option
                      v-for="item in listUsers"
                      :value="item.id"
                      :key="item.id"
                      >{{ item.name }}</Option
                    >
                  </Select>
                </Col></Row
              >
              <Row v-if="!isRecive"
                style="margin-top: 20px; font-size: 13px; margin-bottom: 20px"
              >
                <Col class="col col__item form_search" :xs="24" :sm="3" :md="3" :lg="3"
                  ><span>{{ $t("task.deadline") }}</span></Col
                >
                <Col class="col col__item" :xs="24" :sm="8" :md="8" :lg="8">
                  <DatePicker
                    v-model="MyJobSearch.startDate"
                    @on-change="getMyJob()"
                    type="date"
                    :placeholder="$t('task.startTime')"
                    style="width: 100%"
                  ></DatePicker>
                </Col>
                <Col
                  span="3"
                  style="
                    margin-top: 10px;
                    text-align: right;
                    margin-bottom: 10px;
                  "
                  ><span></span
                ></Col>
                <Col class="col col__item" :xs="24" :sm="9" :md="9" :lg="9">
                  <DatePicker
                    v-model="MyJobSearch.endDate"
                    @on-change="getMyJob()"
                    type="date"
                    :placeholder="$t('task.endTime')"
                    style="width: 100%"
                  ></DatePicker>
                <span class="valid-date" v-if="validDateMyJob">Ngày đến phải lớn hơn ngày bắt đầu !</span>
                </Col>
              </Row>

              <hr />
              <Table v-if="!isRecive"
                style="margin-top: 20px"
                class="table-guildines"
                :columns="columnTableMyJob"
                :data="listMyJobs"
              >
                <template slot-scope="{ row }" slot="name">
                  <span
                    v-if="row.status == 0"
                    @click="viewTask(row)"
                    class="name"
                    >{{ row.name }}</span
                  >
                  <span v-else @click="viewTask(row)" class="nameAfterDone">{{
                    row.name
                  }}</span>
                </template>
                <template slot-scope="{ row }" slot="deadline">
                  <span v-if="row.deadline === null">No deadline</span>
                  <span v-else>
                    <span>{{ row.deadline | moment }}</span
                    ><br />
                    <span v-if="!row.inTimelimit" style="color: red"
                      >(quá hạn)</span
                    >
                  </span>
                </template>
                <template slot-scope="{ row }" slot="status">
                  <!-- <span style="cursor: pointer;">{{converStatus(row.status)}}</span> -->
                  <Select
                    v-model="row.status"
                    @on-change="changeStatusForMyJob(row)"
                  >
                    <Option
                      v-for="(item, index) of statusList"
                      :key="index"
                      :value="item.id"
                      >{{ item.name }}</Option
                    >
                  </Select>
                </template>
                <template slot-scope="{ row }" slot="reporter">
                  <span>{{ row.reporter }}</span>
                </template>
                <template slot-scope="{ row }" slot="action">
                  <Icon class="point_icon" v-if="row._index !== 0 && MyJobSearch.status == 0" type="ios-arrow-dropup-circle" @click="up(row)" size="25"  />
                  <Icon class="point_icon" v-if="row._index !== (listMyJobs.length-1) && MyJobSearch.status == 0"  type="ios-arrow-dropdown-circle" @click="down(row)" size="25" />
                  <!-- <p class="detail" @click="viewTask(row)"><u>Chi tiết</u></p> -->
                </template>
              </Table>
            </Card></Col
          ></Row
        >
      </Col>
    </Row>
  </div>
</template>
<script lang="ts">
import moment from "moment";
import {
  Component,
  Vue,
  Inject,
  Emit,
  Prop,
  Watch,
} from "vue-property-decorator";
// import  addtask  from '../../../src/views/task/components/addTask.vue'
@Component({
  name: "task",
  components: {
  },
  filters: {
    moment(value: any) {
      return moment(value).format("HH:mm | DD-MM-YYYY");
    },
  },
})
export default class ListTask extends Vue {
  private statusList = [];
  private isShowAddTask = false;
  private columnTable = [];
  private columnTableMyJob = [];
  private listMyJobs = [];
  private listCreatedJob = [];
  private listUsers = [];
  isShowConfirmDelete = false;
  idDelete: number;
  private data = {};
  isAssign = false;
  isRecive = false;
  private validDateCreate = false;
  private validDateMyJob = false;
  private MyJobSearch: any = {
    startDate: "",
    endDate: "",
    status: 0,
    reporterId: 0,
  };
  private createdJobSearch: any = {
    startDate: "",
    endDate: "",
    status: 0,
    receiverId: 0,
  };
  private formQuickAdd: any = {
    deadline: "",
    description: "",
    name: "",
    status: 0,
    comment: "",
    remind: 30,
    userId: [],
    id: 0,
  }
  hideCreatedJob(){
     this.isAssign = !this.isAssign
  }
  hideMyJob() {
    this.isRecive = !this.isRecive
  }
  async created() {
    this.formQuickAdd = {...this.formQuickAdd}
    this.statusList = [
      { id: 0, name: this.$t("task.todo") },
      { id: 1, name: this.$t("task.done") },
    ];
    let response = await this.$store.dispatch({
      type: "news/getAllUser",
    });
    this.listUsers = response.map((el) => {
      let item: any = {};
      item.name = el.fullName;
      item.id = el.userId;
      return item;
    });
    // Table công việc đi giao
    this.columnTable = [
      {
        title: this.$t("task.name"),
        slot: "name",
      },
      {
        title: this.$t("task.deadline"),
        slot: "deadline",
        align: "center",
      },
      {
        title: this.$t("task.status"),
        slot: "status",
        align: "center",
      },
      {
        title: this.$t("task.receiver"),
        slot: "receiver",
      },
      {
        title: this.$t("task.action"),
        slot: "action",
        align: "center",
      },
    ];
    // Table công việc được giao
    this.columnTableMyJob = [
      {
        title: this.$t("task.name"),
        slot: "name",
      },
      {
        title: this.$t("task.deadline"),
        slot: "deadline",
        align: "center",
      },
      {
        title: this.$t("task.status"),
        slot: "status",
        align: "center",
      },
      {
        title: this.$t("task.assignee"),
        slot: "reporter",
      },
      {
        title: this.$t("task.action"),
        slot: "action",
        align: "center",
      },
    ];
    this.getMyJob();
    this.getCreatedJob();
  }
  converStatus(value: number) {
    let type = "";
    this.statusList.forEach((el) => {
      if (el.id === value) {
        type = el.name;
      }
    });
    return type;
  }
  onCreateNews() {
    this.$router.push({ name: "addTask" }).catch(() => {});
  }
  async confirmDelete(row) {
    this.idDelete = row.id
    this.isShowConfirmDelete = true;
  }
  async deleteTask() {
    await this.$store.dispatch({
      type: "task/delete",
      id: this.idDelete,
    });
    this.isShowConfirmDelete = false;
    this.$Message.info(this.$t('common.deleteDone'));
    this.getMyJob();
    this.getCreatedJob();
  }
  async getMyJob() {
     let data = {}
    // Nếu nhập ngày đến nhỏ hơn ngày từ  --> ko search
    if( this.MyJobSearch.startDate && this.MyJobSearch.endDate) {
      if (Number(this.MyJobSearch.endDate) - Number(this.MyJobSearch.startDate) < 0) {
        this.validDateMyJob = true;
        return;
      }
    }
    if (this.MyJobSearch.endDate) {
     this.MyJobSearch.endDate = new Date(new Date(this.MyJobSearch.endDate).setHours(23, 59, 59, 0)).toISOString()
    }
    if( this.MyJobSearch.reporterId == 0) {
      data = {
              startDate: this.MyJobSearch.startDate,
              endDate: this.MyJobSearch.endDate,
              status: this.MyJobSearch.status,
              reporterId:''}
    } else { data = this.MyJobSearch }
    let responeofMyJob = await this.$store.dispatch({
      type: "task/getAllMyJob",
      data: data,
    });
    this.validDateMyJob = false;
    this.listMyJobs = responeofMyJob;
  }
  async getCreatedJob() {
    let data = {}
    // Nếu nhập ngày đến nhỏ hơn ngày từ  --> ko search
    if( this.createdJobSearch.startDate && this.createdJobSearch.endDate) {
      if (Number(this.createdJobSearch.endDate) - Number(this.createdJobSearch.startDate) < 0) {
        this.validDateCreate = true;
        return;
      }
    }
    if (this.createdJobSearch.endDate) {
      this.createdJobSearch.endDate = new Date(new Date(this.createdJobSearch.endDate).setHours(23, 59, 59, 0)).toISOString()
    }
     if( this.createdJobSearch.receiverId == 0) {
      data = {
              startDate: this.createdJobSearch.startDate,
              endDate: this.createdJobSearch.endDate,
              status: this.createdJobSearch.status,
              receiverId:''}
    } else { data = this.createdJobSearch }
    let responeofCreatedJob = await this.$store.dispatch({
      type: "task/getAllCreatedJob",
      data: data,
    });
    this.validDateCreate = false;
    this.listCreatedJob = responeofCreatedJob;
  }
  viewTask(rowValue) {
    this.$router.push(`/task/myTask/${rowValue.id}`);
  }
  viewTaskcanEdit(rowValue) {
    this.$router.push(`/task/createTask/${rowValue.id}`);
  }
  async changeStatus(rowValue) {
    let data = {
      id: rowValue.id,
      status: rowValue.status,
    };
    this.change(data);
  }
  async changeStatusForMyJob(rowValue) {
    let data = {
      id: rowValue.jobId,
      status: rowValue.status,
    };
    this.change(data);
  }
  async change(data) {
    await this.$store.dispatch({
      type: "task/change",
      data: data,
    });
    this.$Message.info(this.$t('task.changeSuccess'));
    // this.fetchData();
    this.getCreatedJob();
    this.getMyJob();
  }
  async quickAdd() {
    await this.$store.dispatch({
      type: "task/save",
      dataTask: this.formQuickAdd,
    });  
    this.formQuickAdd.name = "";
    this.formQuickAdd.userId = [];
    this.getCreatedJob();
    this.getMyJob();
  }
  up(row){
    let fromId;
    let toId;
    let i;
    for (i = 0; i < this.listMyJobs.length; i++){
      if( this.listMyJobs[i].id === row.id) {
        fromId = this.listMyJobs[i].id;
        toId = this.listMyJobs[i-1].id;
      }
    }
    this.changeOder(fromId,toId)
  }
  down(row){
    let fromId;
    let toId;
    let i;
    for (i = 0; i < this.listMyJobs.length; i++){
      if( this.listMyJobs[i].id === row.id) {
        fromId = this.listMyJobs[i].id;
        toId = this.listMyJobs[i+1].id;
      }
    }
    this.changeOder(fromId,toId)
  }
  async changeOder(fromId,toId){
    let data = {
      fromId: fromId,
      toId:toId
    }
    await this.$store.dispatch({
      type: "task/changeOrderJob",
      data: data,
    });  
    this.getMyJob();
  }
  refeshMyJob() {
   this.MyJobSearch = {
      startDate: "",
      endDate: "",
      status: 0,
      reporterId: 0,
    }
  this.getMyJob();
  }
refeshCreated() {
  this.createdJobSearch = {
    startDate: "",
    endDate: "",
    status: 0,
    receiverId: 0,
  };
   this.getCreatedJob();
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.button-create {
  background-color: #2d8cf0;
  margin-bottom: 10px;
  margin-left: 17px;
  cursor: pointer;
}
.button-search {
  background-color: #2d8cf0;
  margin-bottom: 10px;
  margin-left: -17px;
}
.point_icon {
  cursor: pointer;
}
.table-guildines {
  .ivu-table td {
    height: 70px !important;
  }
  .ivu-table {
    th {
      height: 60px !important;
    }
  }
}
.form_search {
  text-align: right;
}
.form_quick{
  margin-left: -15px;
}
.name {
  word-break: break-word;
  overflow: hidden;
  color: #2d8cf0;
  cursor: pointer;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 3;
  font-size: 13px;
  line-height: 2em;
  max-height: 78px;
  // text-decoration-line: line-through
}
.nameAfterDone {
  overflow: hidden;
  color: #2d8cf0;
  cursor: pointer;
  text-overflow: ellipsis;
  display: -webkit-box;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 3;
  font-size: 13px;
  line-height: 2em;
  max-height: 78px;
  text-decoration-line: line-through;
}
.delete {
  color: indianred;
  cursor: pointer;
}
.valid-date {
  color: red;
}
@media only screen and (max-width: 768px) { 
.form_search {
  text-align: left;
  margin-bottom: 5px;
  margin-top:5px;
}
.form_quick{
  margin-left: -15px;
  margin-bottom: 10px;
}
}
</style>

