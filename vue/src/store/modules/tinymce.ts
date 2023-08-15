import {Store,Module,ActionContext} from "vuex";
import ListModule from "./list-module";
import ListState from "./list-state";
import TinyMce from "../entities/tinymce";
import Ajax from "../../lib/ajax";
interface TinyMceState extends ListState<TinyMce>{
    editTenant:TinyMce;
}
class TinyMceModule extends ListModule<TinyMceState,any,TinyMce>{
    actions={
    	async uploadImage(context:ActionContext<TinyMceState,any>,payload:any){
    		return await Ajax.post("/api/services/app/Image/UploadImage",payload.data);
    	}
    };
}
const tinyMceModule=new TinyMceModule();
export default tinyMceModule;