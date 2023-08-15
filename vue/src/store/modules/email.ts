import {Store,Module,ActionContext} from 'vuex'
import ListModule from './list-module'
import ListState from './list-state'
import TinyMce from '../entities/tinymce'
import Ajax from '../../lib/ajax'
interface EmailState extends ListState<any>{
}
class EmailModule extends ListModule<EmailState,any,any>{
    actions={
        async getEmailSetting(context:ActionContext<EmailState,any>, payload:any){
            let reponse = await Ajax.get('/api/services/app/EmailSetting/Get');
            return reponse.data.result
        },

        async changeEmailSetting(context:ActionContext<EmailState,any>, payload:any){
            let reponse = await Ajax.post('/api/services/app/EmailSetting/Change', payload.data);
            return reponse.data.result
        },

        async getSettingKomu(context:ActionContext<EmailState,any>, payload:any){
            let reponse = await Ajax.get('/api/services/app/EmailSetting/GetSettingKomu');
            return reponse.data.result
        },

        async changeSettingKomu(context:ActionContext<EmailState,any>, payload:any){
            let reponse = await Ajax.post('/api/services/app/EmailSetting/ChangeSettingKomu', payload.data);
            return reponse.data.result
        },
        async getClientId(context:ActionContext<EmailState,any>, payload:any){
            let reponse = await Ajax.get('/api/services/app/Configuration/GetClientId');
            return reponse.data.result
        },

        async changeClientId(context:ActionContext<EmailState,any>, payload:any){
            let reponse = await Ajax.post('/api/services/app/Configuration/ChangeClientId', payload.data);
            return reponse.data.result
        },

    };
}
const emailModule = new EmailModule();
export default emailModule;