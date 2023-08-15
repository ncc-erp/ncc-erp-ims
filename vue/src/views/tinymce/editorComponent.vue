<template>
  <div class="container">
    <editor
      v-model="editorContent"
      api-key="i68nshmn2313ig8km52u3b20azi6i5ufdmx7dmnt25jfrrj4"
      :init="{
        
         height: 500,
         entity_encoding :'raw',
         menubar: true,
         plugins: [
           'advlist autolink lists link image charmap print preview anchor',
           'searchreplace visualblocks code fullscreen',
           'insertdatetime media table paste code help wordcount pageembed'
         ],
         toolbar:
           'undo redo | formatselect | fontsizeselect | bold italic underline backcolor forecolor| \
           alignleft aligncenter alignright alignjustify | \
           bullist numlist outdent indent | removeformat | link image pageembed code | help',
          images_upload_handler: (blobInfo, success, failure, progress) => {
            handleUploadImg(blobInfo, success, failure, progress)
          },
          images_upload_url: 'postAcceptor.ts',
          
          extended_valid_elements : 'iframe[src|frameborder|style|scrolling|class|width|height|name|align]'
       }"
      @onChange="emitValue"
    />
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Emit, Prop, Watch } from 'vue-property-decorator'
import iView from 'iview'
import Editor from '@tinymce/tinymce-vue'
import appconst from '../../lib/appconst'
@Component({
  components: {
    'editor': Editor
  }
})
export default class EditorComponent extends Vue {
  @Prop() value: any
  private editorContent: any = ''
  
  created() {
  }

  @Watch('value')
  setContent() {
    this.editorContent = this.value
  }

  @Emit('onChange') emitValue() {
    return this.editorContent
  }

  async handleUploadImg(blobInfo, success, failure, progress) {
    let formData = new FormData();
    let fileName: string = blobInfo.filename();
    const extension = fileName.slice(fileName.lastIndexOf('.') + 1);
    const newFileName = `${fileName.slice(0, fileName.lastIndexOf('.'))}-${new Date().getTime()}.${extension}`;
    
    formData.append('file', blobInfo.blob(), newFileName);
    let response = await this.$store.dispatch({
      type: 'tinymce/uploadImage',
      data: formData
    })
    if(response.data.success){
      success(appconst.remoteServiceBaseUrl + response.data.result)
    }
    else failure('Invalid JSON: ' + response.data.error);
  }

}
</script>
<style scoped>
</style>

