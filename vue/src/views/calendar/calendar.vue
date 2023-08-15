<template>
  <div class="container">
    <Calendar/>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
import Calendar from '../../components/fullCalendar/calendar.vue'
import SessionStorageInfoPage from '../../types/session-storage/session-storage'
@Component({
  components: {
    Calendar
  }
})
export default class UserCalendar extends Vue {
  created() {
    SessionStorageInfoPage.clear('New');  
    SessionStorageInfoPage.clear('Event');
    SessionStorageInfoPage.clear('Policy');
    SessionStorageInfoPage.clear('GuildLine');
  }
}
</script>
<style scoped>
</style>

