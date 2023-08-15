import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Role from '../entities/role'
import Guide from '../entities/guide'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface GuideState extends ListState<Guide> {

}
class GuideMutations extends ListMutations<User>{

}
class GuideModule extends ListModule<GuideState, any, Guide>{
    state = {
        title: '',
        typeName: '',
        pageNumber: 1,
        pageSize: 4,
        list: new Array<Guide>(),
        loading:false,
        currentPage:1,
        totalCount: 0
    }
    actions = {
        async getAllGuides(context:ActionContext<GuideState,any>, payload:any){
            let reponse = await Ajax.post('/api/services/app/GuildLines/GetAllPaging',payload.data)
            return reponse.data.result
        },

        async getGuide(context: ActionContext<GuideState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/GuildLines/GetGuildLine?Id='+payload.id)
            return response.data.result
        },

        async createEditGuide(context: any, payload: any){
            let response = await Ajax.post('/api/services/app/GuildLines/Save', payload.dataGuide);
            return response.data.result
        },
        
        async getTotalLike(context: any, payload: any){
            let response = await Ajax.get('/api/services/app/GuildLines/TotalLikeOrDislikeGuideLine?GuideLineId='+payload.id)
            return response.data.result
        },

        async sendLikePost(context: any, payload: any){
            let response = await Ajax.get('/api/services/app/GuildLines/LikeOrDislikeGuideLines?GuideLineId='+payload.id)
            return response.data.result
        },

        async deleteGuide(context: any, payload: any){
            await Ajax.delete('/api/services/app/GuildLines/Delete?Id='+payload.idDelete);
        },
        
        async getEntityType(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/EntityType/GetByEntity?entity='+payload.entity);
            return reponse.data
        },

    };
    mutations = {
        setCurrentPage(state: GuideState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: GuideState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const eventModule = new GuideModule();
export default eventModule;