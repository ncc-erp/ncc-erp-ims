import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Role from '../entities/role'
import Event from '../entities/event'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface EventState extends ListState<Event> {

}
class EventMutations extends ListMutations<User>{

}
class EventModule extends ListModule<EventState, any, Event>{
    state = {
        title: '',
        typeName: '',
        pageNumber: 1,
        pageSize: 4,
        list: new Array<Event>(),
        loading:false,
        currentPage:1,
        totalCount: 0
    }
    actions = {
        async getAllEvents(context:ActionContext<EventState,any>, payload:any){
            let reponse = await Ajax.post('/api/services/app/Events/GetAllPaging',payload.data)
            return reponse.data.result
        },

        async getEvent(context: ActionContext<EventState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/Events/GetEvent?Id='+payload.id)
            return response.data.result
        },
        
        async getTotalLike(context: ActionContext<EventState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/Events/TotalLikeOrDislikeEvent?EventId='+payload.id)
            return response.data.result
        },

        async sendLikePost(context: ActionContext<EventState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/Events/LikeOrDislikeEvents?EventId='+payload.id)
            return response.data.result
        },

        async createEditEvent(context: any, payload: any){
            let response = await Ajax.post('/api/services/app/Events/Save', payload.dataEvent);
            return response.data.result
        },

        async deleteEvent(context: any, payload: any){
            await Ajax.delete('/api/services/app/Events/Delete?Id='+payload.idDelete);
        },
        
        async getEntityType(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/EntityType/GetByEntity?entity='+payload.entity);
            return reponse.data
        },

    };
    mutations = {
        setCurrentPage(state: EventState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: EventState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const eventModule = new EventModule();
export default eventModule;