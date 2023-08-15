<template>
  <div class="task"> 
    <Row >
      <Col class="col col__item" :xs="24" :sm="24" :md="24" :lg="24">
        <Row>
          <Col :xs="24" :sm="24" :md="24" :lg="24"> 
              <h4>Công việc của tôi</h4>
              <hr />      
              <Table 
                style="margin-top: 20px"
                class="table-guildines"
                :columns="columnTableMyJob"
                :data="lisJobs"
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
                <template slot-scope="{ row }" slot="description">
                  <span>{{ row.description}}</span>
                </template>
              </Table>
            </Col
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
export default class PopUpTask extends Vue {
  private statusList = [];
  private columnTableMyJob = [];
  private listUsers = [];
  private lisJobs = [];
  private data = {};
 
  async created() {
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
        title: this.$t("task.description"),
        slot: "description",
      },
    ];
    this.getJobForDashBoard();
  }
 
  async getJobForDashBoard() {
  let respone = await this.$store.dispatch({
      type: "task/viewJobInDashboard",
    });
    this.lisJobs = respone;
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
 
  viewTask(rowValue) {
    this.$router.push(`/task/myTask/${rowValue.id}`);
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
    // this.fetchData();
   this.getJobForDashBoard();
  }
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";

.button-search {
  background-color: #2d8cf0;
  margin-bottom: 10px;
  margin-left: -17px;
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
.name {
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
</style>

