<template>
  <div >
    <ListTask/> 
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
import  addtask  from '../../../src/views/task/components/addTask.vue'
import ListTask from  '../../../src/views/task/components/listTask.vue'
   
@Component({ 
  name: 'task',
  components: {
      addtask, ListTask
  },
})
export default class Task extends Vue {
}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>

