import Vue from "vue";
import Vuex from "vuex";
Vue.use(Vuex);
import app from "./modules/app";
import session from "./modules/session";
import account from "./modules/account";
import user from "./modules/user";
import role from "./modules/role";
import tenant from "./modules/tenant";
import tinymce from "./modules/tinymce";
import policy from "./modules/policy";
import news from "./modules/news";
import blog from "./modules/blog";
import bloguser from "./modules/blog-of-user";
import i18n from "./modules/i18n";
import calendar from "./modules/calendar";
import event from "./modules/event";
import guide from './modules/guide';
import dashboard from './modules/dashboard';
import email from './modules/email';
import task from './modules/task';
import unlockTimeSheet from './modules/unlock-timesheet';
import traditionalRoom from './modules/traditional-room';
const store = new Vuex.Store({
	state: {
		//
	},
	mutations: {
		//
	},
	actions: {

	},
	modules: {
		app,
		session,
		account,
		user,
		role,
		tenant,
		tinymce,
		policy,
		news,
		i18n,
		calendar,
		event,
		guide,
		dashboard,
		blog,
		bloguser,
		email,
		task,
		unlockTimeSheet,
		traditionalRoom
	}
});

export default store;