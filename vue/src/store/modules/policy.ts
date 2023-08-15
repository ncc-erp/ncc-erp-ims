import {Store,Module,ActionContext} from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import TinyMce from '../entities/tinymce'
import Ajax from '../../lib/ajax'
interface PolicyState extends ListState<any>{
    // editTenant:TinyMce;
}
class PolicyModule extends ListModule<PolicyState,any,any>{
    actions={
        async createPolicy(context:ActionContext<PolicyState,any>, payload:any){
            return await Ajax.post('/api/services/app/Policies/Save',payload.data);
        },

        async getPolicyById(context:ActionContext<PolicyState,any>, payload:any){
            let reponse = await Ajax.get('/api/services/app/Policies/GetPolicy?id='+payload.id);
            return reponse.data.result
        },

        async getAllPolicies(context:ActionContext<PolicyState,any>, payload:any){
            let reponse = await Ajax.post('/api/services/app/Policies/GetAllPaging',payload.data)
            return reponse.data.result
        },
        
        async getTotalLike(context: any, payload: any){
            let response = await Ajax.get('/api/services/app/Policies/TotalLikeOrDislikePolicy?PolicyId='+payload.id)
            return response.data.result
        },

        async sendLikePost(context: any, payload: any){
            let response = await Ajax.get('/api/services/app/Policies/LikeOrDislikePolicy?PolicyId='+payload.id)
            return response.data.result
        },

        async deletePolicy(context:ActionContext<PolicyState,any>, payload:any){
            let reponse = await Ajax.delete('/api/services/app/Policies/Delete?id=' + payload.policyId)
            return reponse
        },
        
        async getEntityType(context: any, payload: any){
            let reponse = await Ajax.get('/api/services/app/EntityType/GetByEntity?entity='+payload.entity);
            return reponse.data
        },
        
    };
}
const policyModule = new PolicyModule();
export default policyModule;