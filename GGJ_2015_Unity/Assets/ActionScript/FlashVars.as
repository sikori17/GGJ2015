package {
	import com.unity.UnityNative; 
	import flash.display.LoaderInfo;

	public class FlashVars {
		public static function Join(delimiter: String): String {
			var parameters: Object = LoaderInfo(UnityNative.stage.loaderInfo).parameters;
			var text: String = "";
			for (var key: String in parameters)
				text += (text.length ? delimiter : "") + key + "=" + parameters[key];
			return text;
		}
	}
}