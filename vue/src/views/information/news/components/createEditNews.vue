<template>
  <div class="create-new">
    <h2>{{!isEdit ? $t("news.createNews") : $t("news.editNews")}}</h2>
    <Divider />
    <Row v-if="(isEdit && formCreateNew.canDelete) || (SessionStore.state.user.role == 'Admin' && isEdit)" class="form-row mb-2">
      <Col span="2" offset="22">
        <Button class="btn btn--delete" @click="deleteNews" style="float: right;">{{ $t("policy.delete")}}</Button>
      </Col>
    </Row>
    <Row>
      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.title")}}
          </Col>
          <Col span="20" class="col col__item">
            <Input v-model="formCreateNew.title" />
            <span
              v-if="!formCreateNew.title && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.title")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.shortDescription")}}
          </Col>
          <Col span="20" class="col col__item">
            <Input
              v-model="formCreateNew.shortDescription"
              :maxlength="500"
              type="textarea"
              :autosize="{minRows: 2,maxRows: 3}"
            />
            <span
              v-if="!formCreateNew.shortDescription && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.shortDescription")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.avatar")}}
          </Col>
          <Col span="20" class="col col__item">
            <input class="input-file" type="file" @change="handleInputFile" accept="image/*" />
            <p v-if="editIMG">{{ editIMG }}</p>
          </Col>
        </Row>
      </Col>

       <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("common.coverImage")}}
          </Col>
          <Col span="20" class="col col__item">
              <input type="file" @change="croppie"/>
              <span v-if="!urlIMG">
                <div v-if="isCoverImage">
                  <img v-bind:src="cropped">
                </div>
                <div v-else><vue-croppie ref="croppieRef" :enableOrientation="true" :boundary="{ width: 890, height: 300}" :viewport="{ width:835, height:150, 'type':'square' }"
                @result="result">
                </vue-croppie></div>
                <div align="center">
                <Button v-if="isCrop && !isCoverImage" @click="crop" type="primary">{{ $t("common.crop")}}</Button>
                </div>
              </span>
              <span v-if="urlIMG"><img :src="urlIMG"></span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.description")}}
          </Col>
          <Col span="20" class="col col__item">
            <Editor :value="formCreateNew.description" @onChange="handleDescriptionChange" />
            <span
              v-if="!formCreateNew.description && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.description")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.type")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select v-model="formCreateNew.entityTypeId">
              <Option v-for="item in selectType" :value="item.id" :key="item.id">{{ item.typeName }}</Option>
            </Select>
            <span
              v-if="!formCreateNew.entityTypeId && isClickSubmit"
              class="error-text"
            >{{$t("common.errorEmpty", { field: $t("policy.type")})}}</span>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.group")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select @on-change="changeGroupId()" v-model="groupId">
              <Option v-for="item in selectGroup" :value="item.value" :key="item.value">{{ item.label }}</Option>
            </Select>
          </Col>
        </Row>
      </Col>

      <Col class="col" span="24">
        <Row class="row mb-2">
          <Col span="4" class="col col-label text-right col__item title-left-dd">
            {{ $t("policy.priority")}}
          </Col>
          <Col span="8" class="col col__item">
            <Select v-model="formCreateNew.piority">
              <Option v-for="item in priorities" :value="item.value" :key="item.value">{{ item.label }}</Option>
            </Select>
          </Col>
        </Row>
      </Col>
      <Col class="col" span="24">
        <div class="text-center mt-4">
          <Button class="btn btn--create" @click="createEditNew">
            <p v-if="!isEdit">{{ $t("news.create")}}</p>
            <p v-if="isEdit">{{ $t("news.edit")}}</p>
          </Button>
          <Button class="btn btn--back ml-2" @click="cancelModule">{{ $t("news.cancel")}}</Button>
        </div>
      </Col>
    </Row>

  </div>
</template>
<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import NewsModule from "@/store/modules/news";
import { head } from 'lodash'
import { ISelectType, IFormCreateNew } from '@/types/information/createEditNew'
import editorComponent from '../../../tinymce/editorComponent.vue'
import { EStatus } from '../../../../types/information/EStatus'
import SessionStore from '../../../../store/modules/session'
import url from '../../../../lib/url';
import VueCroppie from 'vue-croppie'
import 'croppie/croppie.css'

Vue.config.productionTip = false
Vue.use(VueCroppie)
@Component({
  components: {
    'Editor': editorComponent
  }
})
export default class CreateEditNews extends Vue {
  private selectType = [] as ISelectType[]
  private formCreateNew = {} as IFormCreateNew
  private loadingStatus: boolean = false
  private groupId: number = 0
  private isEdit: boolean = false
  private editIMG: string = ''
  private coverImage: string = ''
  private EStatus = EStatus
  private isClickSubmit: boolean = false;
  private SessionStore = SessionStore
  private isCoverImage: boolean = false
  private croppieImage: string = ''
  private isCrop: boolean = false
  private cropped: any = null
  private urlIMG: string = ''

  async crop() {
      let options = {
        type: 'base64',
        size: { width: 1635, height: 300 },
        format: 'jpeg'
      };
      let ref: any = this.$refs.croppieRef
      await ref.result(options, (output) => {
        this.cropped = output
      })
        this.isCoverImage = true
        this.formCreateNew.coverImage = this.cropped.replace("data:image/jpeg;base64,","");
  }

  result (output) {
      this.formCreateNew.coverImage = output
    }

  croppie (e) {
    this.isCoverImage = false
    this.isCrop = true
      let files = e.target.files || e.dataTransfer.files;
      if (!files.length) return;
 
      let reader = new FileReader();
      reader.onload = e => {
        let ref: any = this.$refs.croppieRef
        ref.bind({
          url: e.target.result
        });
      };
 
    reader.readAsDataURL(files[0]);
    this.urlIMG = ''
    }

  private selectGroup = [
    {
      value: 0,
      label: "Tất cả"
    }
    ]
  private priorities: any = [
    {
      value: 1,
      label: 'Quan trọng'
    },
    {
      value: 2,
      label: 'Cao'
    },
    {
      value: 3,
      label: 'Thấp'
    }
  ];

  private changeGroupId() {
    this.formCreateNew.groupId = []
    this.formCreateNew.groupId.push(this.groupId)
  }

  handleDescriptionChange(value: string) {
    this.formCreateNew.description = value
  }

  async handleInputFile(event: any) {
    this.formCreateNew.image = event.target.files[0]
    if (this.formCreateNew.image) {
      this.editIMG = ''
    }
  }

  private async createEditNew() {
    this.isClickSubmit = true
    if (this.checkValidate) {
      let dataParam = this.formCreateNew
      if (!this.isEdit) {
        dataParam.status = this.EStatus.Draft
      }
      let formData = new FormData()
      for (let key in dataParam) {
        if (key != 'groupId')
          if (key === 'coverImage') {
            if (dataParam[key]) {
              formData.append(key, dataParam[key]);
            }
          } else formData.append(key, dataParam[key]);
        else {
          dataParam.groupId.map(group => {
            formData.append('groupId', group.toString())
          })
        }
      }
      const reponse = await this.$store.dispatch({
        type: "news/createEditNew",
        dataNews: formData
      })
      this.isClickSubmit = false
      if (this.isEdit) {
        this.$router.push(`/information/news/${this.$route.params.newsId}`)
      } else this.$router.push(`/information/news/${reponse.id}`)
    }
  }

  async beforeMount() {
    const entity: string = 'New'
    const entityType = await this.$store.dispatch({
      type: "news/getEntityType",
      entity: entity
    })
    this.selectType = entityType.result
    if (!this.$route.params.newsId) {
      this.formCreateNew.entityTypeId = head(this.selectType).id
      this.changeGroupId()
      this.formCreateNew.piority = 1
    } else this.mapformCreateNew()
  }

  private async mapformCreateNew() {
    if (this.$route.params.newsId) {
      this.isEdit = true
      const response = await this.$store.dispatch({
        type: "news/getNews",
        id: this.$route.params.newsId
      })
      for (let key in response) {
        if (key != 'image')
          this.formCreateNew[key] = response[key]
      }
      this.groupId = response.groupId[0]
      this.formCreateNew = { ...this.formCreateNew }
      this.formCreateNew.coverImage = null
      this.editIMG = response.image
      this.urlIMG = url + response.coverImage
    }
  }

  cancelModule() {
    if (!this.isEdit) {
      this.$router.push('/information/news')
    } else this.$router.push(`/information/news/${this.$route.params.newsId}`)
  }

  async deleteNews() {
    await this.$store.dispatch({
      type: "news/deleteNews",
      listId: Number(this.$route.params.newsId)
    })
    await this.$router.push('/information/news')
  }

  get checkValidate() {
    let validated = true;
    if (
      this.formCreateNew.groupId != [0] &&
      !this.formCreateNew.title ||
      !this.formCreateNew.piority ||
      !this.formCreateNew.groupId ||
      !this.formCreateNew.entityTypeId ||
      !this.formCreateNew.description ||
      !this.formCreateNew.shortDescription
    ) {
      validated = false;
    }
    return validated;
  }

}
</script>
<style lang="scss" scoped>
@import "src/assets/style/_variable.scss";
</style>


