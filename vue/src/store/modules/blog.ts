import { Store, Module, ActionContext } from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import User from '../entities/user'
import Blog from '../entities/news'
import Ajax from '../../lib/ajax'
import PageResult from '@/store/entities/page-result';
import ListMutations from './list-mutations'

interface BlogState extends ListState<Blog> {

}
class BlogMutations extends ListMutations<User>{

}
class BlogModule extends ListModule<BlogState, any, Blog>{
    state = {
        title: '',
        typeName: '',
        pageNumber: 1,
        pageSize: 4,
        list: new Array<Blog>(),
        loading:false,
        currentPage:1,
        totalCount: 0
    }
    actions = {
        async getAllBlogFeed(context: ActionContext<BlogState, any>, payload: any) {
            context.state.loading = true;
            let reponse = await Ajax.post('/api/services/app/BlogFeed/GetAllBlogFeed', payload.data);
            context.state.loading = false;
           // let page = reponse.data.result as PageResult<User>;
            return reponse.data.result
        },

        async createBlogPost(context:any, payload: any) {
            let reponse = await Ajax.post('/api/services/app/BlogFeed/CreateBlogPost', payload.dataBlog);
            return reponse.data.result
        },

        async detailAPost(context: ActionContext<BlogState, any>, payload: any){
            let response = await Ajax.post('/api/services/app/BlogFeed/DetailAPost?PostId='+payload.id)
            return response.data.result
        },

        async editBlogPost(context:any, payload: any) {
            let reponse = await Ajax.post('/api/services/app/BlogFeed/EditBlogPost', payload.dataBlog);
            return reponse.data.result
        },

        async deleteBlogPost(context: any, payload: any){
            await Ajax.delete('/api/services/app/BlogFeed/DeleteBlogPost?Id='+payload.idDelete);
        },

        async reactionPost(context: any, payload:any){
            let reponse = await Ajax.get('/api/services/app/BlogFeed/ReactionPost?reaction='+payload.data.reaction+'&PostId='+payload.data.postId)
            return reponse.data.result
        },
       
        async findHashtag(context:any,payload:any){
            let reponse =  await Ajax.get('/api/services/app/BlogFeed/FindHashtag',{params:payload.data});
            return reponse.data.result
        },

        async top10Hashtag(context:any,payload:any){
            let reponse =  await Ajax.get('/api/services/app/BlogFeed/Top10Hashtag');
            return reponse.data.result;
        },
    
    };
    mutations = {
        setCurrentPage(state: BlogState, page: number) {
            state.currentPage = page;
        },
        setPageSize(state: BlogState, pagesize: number) {
            state.pageSize = pagesize;
        },
    }
}
const blogModule = new BlogModule();
export default blogModule;