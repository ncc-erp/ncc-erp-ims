<template>
  <div class="addtask">
    <div v-if="!isEdit && !isDetailOfCreate" class="title">{{ $t("task.addTask") }}</div>
    <div v-else class="title">{{ $t("task.editTask") }}</div>
     <Row>
      <Col span="23">
        <Button
          v-if="!isEdit"
          type="primary"
          @click="save"
          style="float: right; margin-right: 5px"
          >{{ $t("task.save") }}</Button
        >
      </Col>
      <Col span="1"  v-if="!isEdit">
        <Button type="default" @click="cancel">{{ $t("task.cancel") }}</Button>
      </Col>
      <Col v-if="isEdit" span="24" style="text-align: right" >
        <Button type="default" @click="cancel">{{ $t("task.cancel") }}</Button>
      </Col>
    </Row>
    <Row>
      <Col span="24" class="task-name">
        <Row class="row mb-2">
          <Col span="4" class="col col-label col__item form_add">
            <span>{{ $t("task.name") }}<span style="color: red">(*)</span></span></Col
          >
          <Col span="20" class="col col__item">
              <Input
                v-model="task.name"       
                class="input_add"
                type="textarea"
                :disabled="isdisable"
                @on-change="changeName()"
              />
              <span v-if="isInvalid" style="color: red">Bạn phải nhập đầy đủ nội dung công việc !</span>
              </Col
        ></Row>
      </Col>
<!----->
  <Col span="24" class="task-status">
        <Row class="row mb-2">
          <Col span="4" class="col col-label col__item form_add">
            <span>{{ $t("task.receiver") }} <span style="color: red">(*)</span></span></Col
          >
          <Col span="8" class="col col__item">
             <Select 
               @on-change="changeUser()" 
               class="input_add" 
               v-model="task.userId" 
               multiple 
               :disabled="isdisable"  
               style="width: 100%"
               filterable
               clearable>
              <Option 
                v-for="item in listUsers"
                :value="item.id"
                :key="item.id"
                >{{ item.name }}</Option
              >
            </Select>
              <span v-if="isInvaliduser" style="color: red">Bạn phải chọn người nhận !</span>
          </Col></Row></Col>
<!----->
      <Col span="24" class="task-status">
        <Row class="row mb-2">
          <Col span="4" class="col col-label col__item form_add">
            <span>{{ $t("task.deadline") }}</span></Col
          >
          <Col span="8" class="col col__item">
            <DatePicker
              class="input_add"
              v-model="task.deadline"
              type="datetime"
              style="width: 100%"
              :disabled="isdisable"
            ></DatePicker
          ></Col>
          
        </Row>
      </Col>

      <Col span="24" class="task-status">
        <Row class="row mb-2">
          <Col span="4" class="col col-label col__item form_add">
            <span>{{ $t("task.status") }}</span></Col
          >
          <Col span="8" class="col col__item">
            <Select 
               class="input_add" 
               v-model="task.status"  
               @on-change = "changeStatus()"
               style="width: 100%">
              <Option
                v-for="item in statusList"
                :value="item.id"
                :key="item.id"
                >{{ item.name }}</Option
              >
            </Select></Col
          ></Row
        >
      </Col>

      <Col span="24" class="task-status">
        <Row class="row mb-2">
          <Col span="4" class="col col-label col__item form_add">
            <span>{{ $t("task.description") }}</span></Col
          >
          <Col span="20" class="col col__item">
            <Input
              v-model="task.description"
              :rows="4" 
              class="input_add"
              type="textarea"
              :disabled="isdisable"
            /> </Col
        ></Row>
      </Col>

      <Col span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label col__item form_add">
            <span>{{ $t("task.timetoNote") }}</span>
            <p> (trước)</p></Col
          >
          <Col span="4" class="col col__item">
            <Input
              class="input_add"
              type="number"
              v-model="task.remind"
              style="width: 100%"
            /></Col>
            <Col span="3" class="col col__item">
            <Select 
                 class="input_add" 
                 v-model="time" 
                 style="width: 100%"
            >
              <Option
                v-for="item in remiderTimes"
                :value="item.id"
                :key="item.id"
                >{{ item.name }}</Option>
            </Select></Col>
            <Col span="3" class="col col__item" style="margin-top:8px">
             <Button
                v-if="isEdit"
                type="primary"
                @click="setRemider"
             >{{ $t("task.setReminder") }}</Button
             >
            </Col>
        </Row>
      </Col>
    </Row>
    <hr />
   
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
import SessionStore from "../../../store/modules/session";
import { Mentionable } from "vue-mention";
import moment from "moment";

@Component({
  name: "task",
  components: { Mentionable },
})
export default class AddTask extends Vue {
  private statusList = [];
  private remiderTimes = [];
  private SessionStore = SessionStore;
  private items: any = [];
  private listUserIds = [];
  private listUsers = [];
  private minutes :number = 30;
  private time : number = 0;
  private isdisable = false;
  private isInvalid = false;
  private isInvaliduser = false;
  private isEdit = false;
  private isDetailOfCreate = false;
  private task: any = {
    deadline: "",
    description: "",
    name: "",
    status: 0,
    comment: "",
    remind: 30,
    userId: [],
    id: 0,
  };
  searchStatus: string = "";

  async created() {
    this.fetchData();
    this.task = { ...this.task };
    let response = await this.$store.dispatch({
      type: "news/getAllUser",
    });
     this.listUsers = response.map((el) => {
      let item: any = {};
      item.name = el.fullName;
      item.id = el.userId;
      return item;
    });
    if( this.$route.name == 'createTask') {
      this.isDetailOfCreate = true;
      let response = await this.$store.dispatch({
      type: "task/getCreatedJob",
      id: this.$route.params.jobId
      });
      this.task = {
        deadline: response.deadline,
        description: response.description,
        name: response.name,
        status: response.status,
        comment: response.comment,
        remind: response.remind,
        creationTime: response.creationTime,
        userId: [],
        id: response.id
     }
      response.users.forEach(element => {
        this.task.userId.push(element.userId)
      });
    }
    if( this.$route.name == 'myTask') {
      this.isEdit = true;
      this.isDetailOfCreate = true;
      this.isdisable = true;
      let response = await this.$store.dispatch({
      type: "task/getMyJob",
      id: this.$route.params.jobId
       });
       this.task = response
       this.task.userId = []
       response.receiver.forEach(element => {
        this.task.userId.push(element.userId)
      });
    }
  }
  fetchData() {
    this.statusList = [
      { id: 0, name: this.$t("task.todo") },
      { id: 1, name: this.$t("task.done") },
    ];
    this.remiderTimes = [
      { id: 0, name: this.$t("task.minute") },
      { id: 1, name: this.$t("task.hour") },
    ]
  }
  // beforeMount() {
  //   this.fetchData();
  // }
  changeName() {
    this.isInvalid = false;
  }
  changeUser() {
    this.isInvaliduser = false;
  }

  async changeStatus() {
     if( this.$route.name == 'myTask') {
       let data = {
         id: Number(this.task.jobId),
         status: this.task.status,
        };
      await this.$store.dispatch({
       type: "task/change",
       data: data,
      });
       this.$Message.info(this.$t('task.changeSuccess'));
     } else{ return; }
  
  }

  getDateHasTime(newdate) {
    const m = moment();
    const date = moment(newdate);
    newdate = date.format("YYYY-MM-DD HH:mm:ss");
    return newdate;
  }
  async save() {
    let data = {}
    if (this.task.deadline) {
      const item = this.task.deadline;
      const deadline = this.getDateHasTime(item)
      data = {
         deadline: deadline,
         description: this.task.description,
         name: this.task.name,
         status: this.task.status,
         comment: this.task.comment,
         remind: this.task.remind,
         userId: this.task.userId,
         id: this.task.id,
      }
    } else { data = this.task}
   
    if(!this.task.deadline) {
      this.task.remind = 0;
    }
    if( this.time === 1) {
      this.task.remind = this.task.remind * 60
    }
    if(!this.task.name) { 
      this.isInvalid = true;
      return; 
    }
    if( this.task.userId.length == 0) { this.isInvaliduser = true; return;  }
    await this.$store.dispatch({
      type: "task/save",
      dataTask: data,
    });  
    this.$Message.info(this.$t('common.saveDone'));
    this.cancel();
  }

  async setRemider() {
    if( this.$route.name == 'myTask') {
    let data = {
      minutes: Number(this.task.remind),
      id: Number(this.$route.params.jobId)
    }
     await this.$store.dispatch({
      type: "task/remind",
      data: data,
    });
    this.$Message.info(this.$t('task.setRemindDone'));
    } else { return; }
  }
  cancel() {
    this.$router.push(`/task`)
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
.title {
  font-size: 20px;
  color: #17233d;
  font-weight: bold;
}
.input_add {
  margin-top: 8px;
  width: 100%;
  border: 1px solid #dcdee2;
  border-radius: 4px;
}
.input_add:focus {
    border-color: #57a3f3;
    outline: 0;
    box-shadow: 0 0 0 2px rgb(45 140 240 / 20%);
}
.form_add {
  text-align: right;
}
@media only screen and (max-width: 768px) { 
.form_add {
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
<style>
.ivu-input[disabled],
fieldset[disabled] .ivu-input {
  color: darkslategray
}
.ivu-select-single .ivu-select-selection .ivu-select-selected-value {
  color: darkslategray
}
</style>