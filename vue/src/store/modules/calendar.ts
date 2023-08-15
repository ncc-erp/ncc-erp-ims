import { Store, Module, ActionContext } from "vuex";
import ListModule from "./list-module";
import ListState from "./list-state";
import TinyMce from "../entities/tinymce";
import Ajax from "../../lib/ajax";
import Calendar from "../entities/calendar";
interface CalendarState extends ListState<Calendar> {
  editTenant: TinyMce;
}
class CalendarModule extends ListModule<CalendarState, any, Calendar>{
  actions = {
  	async getCalendar(context: ActionContext<CalendarState, any>, payload: any) {
  		return await Ajax.get("/api/services/app/UserCalendars/GetUserCalendars", {params: payload.data});
    },
  	async saveEventCalendar(context: ActionContext<CalendarState, any>, payload: any) {
  		return await Ajax.post("/api/services/app/UserCalendars/Save", payload.data);
    },
  	async deleteEventCalendar(context: ActionContext<CalendarState, any>, payload: any) {
  		return await Ajax.delete(`/api/services/app/UserCalendars/Delete?id=${payload.id}`);
    }
    
  };
}
const calendarModule = new CalendarModule();
export default calendarModule;