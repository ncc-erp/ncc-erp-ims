<template>
  <div class="full-screen-btn-con language-list" style="width:80px">
    <Dropdown @on-click="changeLanguage">
      <a>
        <i :class="currentLanguage.icon" style="display:inline-block"></i>
        {{currentLanguage.displayName}}
        <Icon type="arrow-down-b"></Icon>
      </a>
      <DropdownMenu slot="list">
        <DropdownItem
          v-for="(language,index) in languages"
          :key="index"
          :name="language.name"
          style="text-align:left"
        >
          <i :class="language.icon" style="display:inline-block"></i>
          {{language.displayName}}
        </DropdownItem>
      </DropdownMenu>
    </Dropdown>
  </div>
</template>
<script lang="ts">
import { Component, Vue, Inject, Prop, Watch } from 'vue-property-decorator';
import Util from '../lib/util'
import AbpBase from '../lib/abpbase'
@Component
export default class LanguageList extends AbpBase {
  private currentLang: string = sessionStorage.getItem('lang')
  private languages: any = [
    {
      displayName: 'English',
      icon: 'famfamfam-flags us',
      isDefault: true,
      isDisabled: false,
      isRightToLeft: false,
      name: 'en'
    },
    {
      displayName: 'Tiếng Việt',
      icon: 'famfamfam-flags vn',
      isDefault: false,
      isDisabled: false,
      isRightToLeft: false,
      name: 'vn'
    }
  ]
  private currentLanguage: any = {
    displayName: 'Tiếng Việt',
    icon: 'famfamfam-flags vn',
    isDefault: false,
    isDisabled: false,
    isRightToLeft: false,
    name: 'vn'
  }
  created() {
    if (this.currentLang) {
      this.currentLanguage = this.languages.find(lang => lang.name === this.currentLang)
    }
  }
  async changeLanguage(languageName: string) {
    await this.$store.dispatch({
      type: 'i18n/setLang',
      data: languageName
    })
    this.currentLanguage = this.languages.find(lang => lang.name === languageName)
  }
}
</script>
<style>
.language-list {
  width: 80px;
}
.language-list i {
  display: inline-block;
}
</style>


