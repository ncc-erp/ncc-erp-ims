import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Role from '../entities/role'
import News from '../entities/news'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface NewsState extends ListState<News> {

}
class NewsMutations extends ListMutations<User>{

}
class NewsModule extends ListModule<NewsState, any, News>{
    state = {
        title: '',
        typeName: '',
        pageNumber: 1,
        pageSize: 4,
        list: new Array<News>(),
        loading:false,
        currentPage:1,
        totalCount: 0
    }
    actions = {
        async getAllNews(context: ActionContext<NewsState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.post('/api/services/app/New/GetAllNews', payload.data);
            context.state.loading = false;
            let page = reponse.data.result as PageResult<User>;

            return reponse.data.result
        },

        async getNews(context: ActionContext<NewsState, any>, payload: any){
            let response = await Ajax.get('/api/services/app/New/GetNew?Id='+payload.id)
            return response.data.result
        },

        async createEditNew(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/New/Save', payload.dataNews);
            return reponse.data.result
        },

        async getEntityType(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/EntityType/GetByEntity?entity='+payload.entity);
            return reponse.data
        },

        async deleteNews(context: any, payload: any){
            await Ajax.delete('/api/services/app/New/DeleteNews?id='+payload.listId);
        },

        async init(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/Session/GetCurrentLoginInformations');
            return reponse.data.result
        },

        async getTotalLike(context: any, payload: any){
            let response = await Ajax.get('/api/services/app/New/TotalLikeOrDislikeNews?NewsId='+payload.id)
            return response.data.result
        },

        async sendLikePost(context: any, payload: any){
            let response = await Ajax.get('/api/services/app/New/LikeOrDislikeNews?NewsId='+payload.id)
            return response.data.result
        },

        async getMainCommentPaging(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Comments/GetMainCommentPaging', payload.mainComment);
            return reponse.data.result
        },

        async getSubCommentPaging(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Comments/GetSubCommentPaging', payload.subComment);
            return reponse.data.result
        },

        async saveMainComment(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Comments/SaveMainComment', payload.dataComment);
            return reponse.data.result
        },

        async saveSubComment(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Comments/SaveSubComment', payload.dataSubComment);
            return reponse.data.result
        },

        async likeMainComment(context: any, payload: any){
            await Ajax.post('/api/services/app/Comments/LikeMainComment?id='+payload.idComment);
        },

        async likeSubComment(context: any, payload: any){
            await Ajax.post('/api/services/app/Comments/LikeSubComment?id='+payload.idSubComment);
        },

        async deleteMainComment(context: any, payload: any){
            await Ajax.delete('/api/services/app/Comments/DeleteMainComment?id='+payload.idComment);
        },

        async deleteSubComment(context: any, payload: any){
            await Ajax.delete('/api/services/app/Comments/DeleteSubComment?id='+payload.idSubComment);
        },

        async getAllUser(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/User/GetAllUser');
            return reponse.data.result
        },

        async GetAllNotification(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/Notification/GetAll');
            return reponse.data.result
        },

        async clickNotification(context: any, payload: any){
            let reponse = await Ajax.post('/api/services/app/Notification/ClickNotification');
            return reponse.data.result
        },

    };
    mutations = {
        setCurrentPage(state: NewsState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: NewsState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const newsModule = new NewsModule();
export default newsModule;