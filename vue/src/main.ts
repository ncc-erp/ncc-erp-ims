import Vue from 'vue';
import App from './app.vue';
import ViewUI from 'view-design';
import locale from 'iview/dist/locale/en-US';
import { router } from './router/index';
import 'famfamfam-flags/dist/sprite/famfamfam-flags.css';
import './theme.less';
import '@fortawesome/fontawesome-free/css/all.css';
import 'hooper/dist/hooper.css';
import '@fortawesome/fontawesome-free/js/all.js';
import Ajax from './lib/ajax';
import Util from './lib/util';
import SignalRAspNetCoreHelper from './lib/SignalRAspNetCoreHelper';
import i18n from './lang/i18n'
import store from './store/index';
import VueSimpleAlert from 'vue-simple-alert';
import { EmojiPickerPlugin } from 'vue-emoji-picker'
import { Mentionable } from "vue-mention";
import { appRouters, otherRouters } from './router/router';
Vue.use(ViewUI, { locale: locale });
Vue.config.productionTip = false;
if (!abp.utils.getCookieValue('Abp.Localization.CultureName')) {
  let language = navigator.language;
  abp.utils.setCookieValue('Abp.Localization.CultureName', language, new Date(new Date().getTime() + 5 * 365 * 86400000), abp.appPath);
}
Vue.use(EmojiPickerPlugin);
Vue.use(VueSimpleAlert);
Vue.use(Mentionable);
Vue.use(require('vue-google-login'))
Vue.component("PopupUnLock", require("./views/extension/unlock-timesheet/components/popup-unlock.vue").default)
export const EventBus = new Vue();
Ajax.get('/AbpUserConfiguration/GetAll').then(data => {
  Util.abp = Util.extend(true, Util.abp, data.data.result);

  new Vue({
    i18n,
    render: h => h(App),
    router: router,
    store: store,
    data: {
      currentPageName: ''
    },
    async mounted() {
      this.currentPageName = this.$route.name as string;
      await this.$store.dispatch({
        type: 'session/init'
      })
      if (!!this.$store.state.session.user && this.$store.state.session.application.features['SignalR']) {
        if (this.$store.state.session.application.features['SignalR.AspNetCore']) {
          SignalRAspNetCoreHelper.initSignalR();
        }
      }
      this.$store.commit('app/initCachepage');
      this.$store.commit('app/updateMenulist');
    },
    created() {
      let tagsList: Array<any> = [];
      appRouters.map((item) => {
        if (item.children.length <= 1) {
          tagsList.push(item.children[0]);
        } else {
          tagsList.push(...item.children);
        }
      });
      this.$store.commit('app/setTagsList', tagsList);
    }
  }).$mount('#app')
})

