<template>
  <div class="full-calendar">
    <full-calendar :options="calendarOptions" ref="calendar" />
    <Modal
      v-model="isShowPopup"
      class="event-popup"
      :title="isClickDate ? $t('calendar.addEvent') : $t('calendar.editEvent')"
      :closable="true"
      @on-visible-change="this.resetFields"
    >
      <p>{{ $t("calendar.time") }}:</p>
      <TimePicker
        :value="this.currentEventTime"
        format="HH:mm"
        placeholder
        style="width: 100px"
        @on-change="this.setCurrentEventTime"
      ></TimePicker>
      <p>{{ $t("calendar.note") }}:</p>
      <div v-if="currentEventTitle && currentEvent.entityId">
        <Input v-model="currentEventTitle">
          <Button type="primary" slot="append" @click="viewDetail">{{
            $t("common.viewDetail")
          }}</Button>
        </Input>
      </div>
      <Input v-model="currentEventTitle" v-else />
      <span v-if="!currentEventTitle && isClickSubmit" class="error-text">{{
        $t("common.errorEmpty", { field: $t("policy.note") })
      }}</span>
      <div
        slot="footer"
        class="modal-footer"
        :class="isClickDate ? 'align-button-center' : ''"
      >
        <Button v-if="!isClickDate" type="error" @click="showDelete">{{
          $t("common.delete")
        }}</Button>
        <div class="right-side">
          <Button type="primary" @click="this.saveEvent">{{
            $t("common.save")
          }}</Button>
          <Button type="default" @click="handleCancel">{{
            $t("common.cancel")
          }}</Button>
        </div>
      </div>
    </Modal>
    <Modal v-model="isShowDelete" :title="$t('common.confirm')" width="300">
      <p>{{ $t("calendar.deleteMessage") }}</p>
      <div slot="footer" style="display: flex; justify-content: center;">
        <Button type="error" @click="this.deleteEvent">{{
          $t("common.delete")
        }}</Button>
        <Button type="default" @click="isShowDelete = false">{{
          $t("common.cancel")
        }}</Button>
      </div>
    </Modal>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop } from "vue-property-decorator";
import FullCalendar from "@fullcalendar/vue";
import interactionPlugin from "@fullcalendar/interaction";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import moment from "moment";
import ICalendar from "../../store/entities/calendar";
import AbpBase from "../../lib/abpbase";
import iView from "iview";
import { CellGroup } from "view-design";
@Component({
  components: {
    FullCalendar,
  },
})
export default class Calendar extends AbpBase {
  @Prop({ type: Boolean, default: false }) private isReadOnly: boolean;
  @Prop({ type: Number, default: 800 }) private height: Number;
  private fullCalendarApi: any = null;
  private isShowDelete: boolean = false;
  private isShowPopup: boolean = false;
  private isClickDate: boolean = false;
  private currentEvent: any = {};
  private currentEventTitle: string = "";
  private currentEventTime: string = "";
  private currentClickedDate: string = "";
  private calendarOptions: any = {
    selectable: true,
    plugins: [dayGridPlugin, interactionPlugin, timeGridPlugin],
    initialView: "dayGridMonth",
    customButtons: {},
    contentHeight: this.height,
    headerToolbar: {
      left: "prevButton,nextButton todayButton title",
      center: "",
      //right: 'monthViewButton,weekViewButton,dayViewButton'
      right: "",
    },
    dateClick: !this.isReadOnly ? this.handleDateClick : null,
    eventClick: !this.isReadOnly ? this.handleEventClick : this.viewDetail,
    events: [],
    views: {
      timeGrid: {
        dayMaxEvents: 3,
        titleFormat: { year: "numeric", month: "2-digit", day: "2-digit" },
      },
      dayGrid: {
        dayMaxEvents: 3,
        titleFormat: { year: "numeric", month: "2-digit" },
      },
    },
  };
  private listEvent: any = [];
  private now: Date = new Date();
  private isClickSubmit: boolean = false;
  created() {
    this.calendarOptions.customButtons = {
      nextButton: {
        text: "",
        click: this.nextView,
        icon: "fc-icon fc-icon-chevron-right",
      },
      prevButton: {
        text: "",
        click: this.prevView,
        icon: "fc-icon fc-icon-chevron-left",
      },
      todayButton: {
        text: this.$t("calendar.today"),
        click: this.moveToToday,
      },
      // monthViewButton: {
      //   text: this.$t('calendar.month'),
      //   click: this.changeToMonthView
      // },
      // weekViewButton: {
      //   text: this.$t('calendar.week'),
      //   click: this.changeToWeekView
      // },
      // dayViewButton: {
      //   text: this.$t('calendar.day'),
      //   click: this.changeToDayView
      // }
    };
  }
  mounted() {
    this.fetchData();
  }

  get currentViewDate() {
    return this.fullCalendarApi.getDate();
  }

  private nextView() {
    this.fullCalendarApi.next();
    this.fetchData();
  }

  private prevView() {
    this.fullCalendarApi.prev();
    this.fetchData();
  }

  private moveToToday() {
    this.fullCalendarApi.today();
    this.fetchData();
  }

  private changeToMonthView() {
    this.fullCalendarApi.changeView("dayGridMonth");
    this.fetchData();
  }

  private changeToWeekView() {
    this.fullCalendarApi.changeView("timeGridWeek");
    this.fetchData();
  }

  private changeToDayView() {
    this.fullCalendarApi.changeView("timeGridDay");
    this.fetchData();
  }

  async fetchData() {
    this.fullCalendarApi = this.$refs.calendar as any;
    this.fullCalendarApi = this.fullCalendarApi.getApi();
    let currentMonth = this.currentViewDate.getMonth();
    let currentYear = this.currentViewDate.getFullYear();
    // this.fromDate = new Date(`${currentYear}-${currentMonth + 1}-1`);
    // this.toDate = currentMonth === 11 ? new Date(`${currentYear + 1}-1-1`) : new Date(`${currentYear}-${currentMonth + 2}-1`);
    const from = moment(this.currentViewDate)
      .startOf("M")
      .format("YYYY-MM-DD");
    const to = moment(this.currentViewDate)
      .add(1, "M")
      .startOf("M")
      .format("YYYY-MM-DD");
    let response = await this.$store.dispatch({
      type: "calendar/getCalendar",
      data: {
        fromDate: from,
        toDate: to,
      },
    });
    this.calendarOptions.events = [];
    response.data.result.map((event: ICalendar) => {
      this.calendarOptions.events.push({
        date: `${moment(event.date).format("YYYY-MM-DD")}${
          new Date(event.date).getHours()
            ? ` ${moment(event.date).format("HH:mm")}`
            : ""
        }`,
        // date: moment(event.date).format('YYYY-MM-DD hh:mm'),
        title: event.note,
        extendedProps: {
          ...event,
        },
      });
    });
  }

  private handleDateClick(arg) {
    this.currentClickedDate = arg.dateStr;
    this.isClickDate = true;
    this.isShowPopup = true;
  }

  private handleEventClick(arg) {
    this.currentEvent = arg.event._def.extendedProps;
    this.currentEventTime = arg.event._def.extendedProps.time
      ? arg.event._def.extendedProps.time
      : "";
    this.currentEventTitle = arg.event._def.title;
    this.isClickDate = false;
    this.isShowPopup = true;
  }

  private setCurrentEventTime(value: any) {
    this.currentEventTime = value;
  }

  private async saveEvent(value: any) {
    this.isClickSubmit = true;
    if (this.currentEventTitle) {
      let date = "";
      if (this.currentClickedDate == "") {
        let dateConvert = new Date(this.currentEvent.date);
        date = `${dateConvert.getFullYear()}-${dateConvert.getMonth() +
          1}-${dateConvert.getDate()} ${this.currentEventTime}`;
      } else {
        date = `${this.currentClickedDate} ${this.currentEventTime}`;
      }
      let response = await this.$store.dispatch({
        type: "calendar/saveEventCalendar",
        data: {
          id: this.isClickDate ? undefined : this.currentEvent.id,
          note: this.currentEventTitle,
          date: date.trim(),
        },
      });
      await this.fetchData();
      this.currentEventTitle = "";
      this.currentClickedDate = "";
      this.currentEventTime = "";
      this.currentEvent = {};
      this.isClickSubmit = false;
      this.isShowPopup = false;
    }
  }

  private showDelete() {
    this.isShowDelete = true;
  }

  private async deleteEvent() {
    let response = await this.$store.dispatch({
      type: "calendar/deleteEventCalendar",
      id: this.currentEvent.id,
    });
    await this.fetchData();
    this.isShowDelete = false;
    this.isShowPopup = false;
  }

  resetFields() {
    if (!this.isShowPopup) {
      this.currentEventTitle = "";
      this.currentClickedDate = "";
      this.currentEventTime = "";
    }
  }

  handleCancel() {
    this.isShowPopup = false;
    this.isClickSubmit = false;
  }

  viewDetail(e) {
    if (this.currentEvent.entityId || e.event.extendedProps.entityId) {
      if (this.currentEvent.url.includes("task")) {
        let routeData = this.$router.resolve({
          name: "myTask",
          params: {
            jobId: this.currentEvent.entityId
              ? this.currentEvent.entityId
              : e.event.extendedProps.entityId,
          },
        });
        window.open(routeData.href, "_blank");
      } else {
        let routeData = this.$router.resolve({
          name: "EventDetail",
          params: {
            eventId: this.currentEvent.entityId
              ? this.currentEvent.entityId
              : e.event.extendedProps.entityId,
          },
        });
        window.open(routeData.href, "_blank");
      }
    }
    return;
  }
}
</script>
<style lang="scss" scoped>
.event-popup {
  p {
    margin: 10px 0;
  }
  .modal-footer {
    display: flex;
    justify-content: space-between;
    > button {
      max-width: fit-content;
    }
    .right-side {
      display: flex;
    }
  }
  .align-button-center {
    display: flex;
    justify-content: center !important;
  }
  .error-text {
    color: red;
  }
}
.fc-toolbar-chunk:first-child {
  display: flex;
  align-items: baseline;
}
.fc-header-toolbar > :first-child {
  display: flex;
  align-items: baseline;
}
</style>
<style lang="scss">
.user-calendar {
  position: relative;
  .full-calendar {
    a {
      color: #111;
    }
    .fc-toolbar-chunk:first-child {
      display: flex;
      align-items: baseline;
    }
    .fc-button {
      // background-image: linear-gradient(to bottom, #37d1e5 , #03c0da);
      border-radius: 15px !important;
      border: none;
      background: indianred;
    }
    .fc-toolbar-title {
      // width: 250px;
      //left: 15px;
      //top: 80px;
      font-size: 15px;
      font-weight: 700;
    }
    .fc-todayButton-button,
    .fc-dayViewButton-button,
    .fc-weekViewButton-button,
    .fc-monthViewButton-button {
      padding-left: 20px !important;
      padding-right: 20px !important;
    }
    .fc-prevButton-button {
      margin-right: 5px;
    }
    .fc-weekViewButton-button {
      margin-left: 5px !important;
      margin-right: 5px;
    }
  }
}
@media only screen and (max-width: 1530px) {
  .user-calendar {
    .full-calendar {
      position: relative;
      .fc-toolbar {
        display: block;
        margin-bottom: 15px;
      }
      .fc-button {
        margin-bottom: 15px;
      }
      .fc-toolbar-title {
        top: 100px;
      }
    }
  }
}
@media only screen and (max-width: 1123px) {
  .user-calendar {
    .full-calendar {
      position: relative;
      .fc-toolbar {
        display: block;
        margin-bottom: 15px;
      }
      .fc-button-group {
        display: none;
      }
      .fc-toolbar-title {
        top: 180px;
        left: 5px;
      }
    }
  }
}
@media only screen and (max-width: 770px) {
  .user-calendar {
    .full-calendar {
      position: relative;
      .fc-toolbar {
        display: block;
      }
      .fc-toolbar-title {
        top: 180px;
      }
    }
  }
}
</style>
