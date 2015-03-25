using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechRecognition;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=391641 を参照してください

namespace SpeechRecognizerApp
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。
        /// このプロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: ここに表示するページを準備します。

            // TODO: アプリケーションに複数のページが含まれている場合は、次のイベントの
            // 登録によりハードウェアの戻るボタンを処理していることを確認してください:
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed イベント。
            // 一部のテンプレートで指定された NavigationHelper を使用している場合は、
            // このイベントが自動的に処理されます。
        }

        private async void DefaultRecognizing_OnClick(object sender, RoutedEventArgs e)
        {
            // カスタム制約なし
            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            await speechRecognizer.CompileConstraintsAsync();

            var result = await speechRecognizer.RecognizeWithUIAsync();

            var dialog = new MessageDialog(result.Text, "Text spoken");
            await dialog.ShowAsync();
        }

        private async void TopicConstraintRecognizing_OnClick(object sender, RoutedEventArgs e)
        {
            // Web 検索文法の指定 (SpeechRecognitionTopicConstraint)

            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            speechRecognizer.RecognitionQualityDegrading += speechRecognizer_RecognitionQualityDegrading;

            var webSearchGrammar = new SpeechRecognitionTopicConstraint(SpeechRecognitionScenario.WebSearch, "webSearch");

            speechRecognizer.UIOptions.AudiblePrompt = "Say what you want to speach for ...";
            speechRecognizer.UIOptions.ExampleText = @"Ex. 'weather for London";
            speechRecognizer.Constraints.Add(webSearchGrammar);

            await speechRecognizer.CompileConstraintsAsync();

            var result = await speechRecognizer.RecognizeWithUIAsync();

            var dialog = new MessageDialog(result.Text, "Text spoken");
            await dialog.ShowAsync();
        }

        private void speechRecognizer_RecognitionQualityDegrading(Windows.Media.SpeechRecognition.SpeechRecognizer sender, Windows.Media.SpeechRecognition.SpeechRecognitionQualityDegradingEventArgs args)
        {
            // throw new NotImplementedException();
        }

        private async void ListConstraintRecognizing_OnClick(object sender, RoutedEventArgs e)
        {
            // プログラムによる一覧の制約の指定 (SpeechRecognitionListConstraint)

            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            string[] responses = {"Yes", "No"};

            var list = new SpeechRecognitionListConstraint(responses, "yesOrNo");

            speechRecognizer.UIOptions.ExampleText = @"Ex. 'yes', 'no'";
            speechRecognizer.Constraints.Add(list);

            await speechRecognizer.CompileConstraintsAsync();

            var result = await speechRecognizer.RecognizeWithUIAsync();

            var dialog = new MessageDialog(result.Text, "Text spoken");
            dialog.ShowAsync();
        }

        private async void GrammarFileConstraintRecognizing_OnClick(object sender, RoutedEventArgs e)
        {
            // SRGS 文法 (SpeechRecognitionGrammarFileConstraint)

            var speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();

            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Sample.grxml"));
            
            var grammarFileCOnstraint = new SpeechRecognitionGrammarFileConstraint(storageFile, "colors");

            speechRecognizer.UIOptions.ExampleText =@"Ex. 'blue background', 'green text'";
            speechRecognizer.Constraints.Add(grammarFileCOnstraint);

            await speechRecognizer.CompileConstraintsAsync();

            var result = await speechRecognizer.RecognizeWithUIAsync();

            var dialog = new MessageDialog(result.Text, "Text spoken");
            dialog.ShowAsync();
        }
    }
}
