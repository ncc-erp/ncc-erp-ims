<template>
    <div class="add-customer">
        <Modal
            v-model="isShow"
            footer-hide
            width="30%">
            
            <div slot="header" align="center">
                <h1>{{$t('common.editComment')}}</h1>
            </div>
            <Input v-model="dataComment.comment" class="comment" type="textarea" :rows="4" placeholder="Enter something..." />
            <Row type="flex" class="code-row-bg" justify="space-around" align="middle">
                <Col span="11" align="right">
                    <Button type="primary"
                        size="large"
                         @click="saveComment"
                    >{{$t('common.save')}}</Button>
                </Col>
                <Col span="12" align="left">
                    <Button type="default"
                        size="large"
                        @click="cancel"
                    >{{$t('common.cancel')}}</Button>
                </Col>
            </Row>
        </Modal>
    </div>
</template>
<script lang="ts">
    import { Component, Vue, Prop, Emit } from 'vue-property-decorator';
    import Util from '@/lib/util'
    import AbpBase from '@/lib/abpbase'
    @Component({
        name: 'editComment',
    })
    export default class editComment extends AbpBase{
      @Prop() private isSubComment: boolean
      @Prop() private dataEditComment: any
      @Prop() private entityName: string
      @Prop() private entityId: string
      private dataComment: any = {}
      private isShow: boolean = true

      beforeMount() {
        this.dataComment = {}
        this.dataComment = this.dataEditComment
      }

      async saveComment() {
        if (this.isSubComment) {
          let params = {
          mainCommentId: this.dataComment.mainId,
          comment: this.dataComment.comment,
          id: this.dataComment.id
          }
          let formData = new FormData()
          for (let key in params) {
              formData.append(key, params[key])
            }
          await this.$store.dispatch({
              type: "news/saveSubComment",
              dataSubComment: formData
          });
        } else {
          let params = {
          entityId: this.entityId,
          entityName: this.entityName,
          comment: this.dataComment.comment,
          id: this.dataComment.id
          }
          let formData = new FormData()
          for (let key in params) {
              formData.append(key, params[key])
            }
          await this.$store.dispatch({
              type: "news/saveMainComment",
              dataComment: formData
            });
        }
        this.cancel()
      }

      cancel () {
          this.$Message.info(this.$t('common.cancel'));
          this.onSubmit()
      }

      @Emit('onSubmit')
      private onSubmit() {
          return
      }
    }
</script>
<style scoped>  
.comment  {
  margin-bottom: 20px;
}
</style>
