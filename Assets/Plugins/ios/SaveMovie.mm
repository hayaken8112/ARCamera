#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVFoundation/AVFoundation.h>


/*
 指定したパスの画像をカメラロールに保存するのに利用するネイティブコード。
 */
// 指定したパスの画像をカメラロールに保存する。
extern "C" void _MovieToAlbum (const char* path, const char *gameObjectName, const char *callbackMethodName)
{

    
    NSURL *videourl = [NSURL fileURLWithPath:[NSString stringWithUTF8String:path]];
    ALAssetsLibrary *library = [[[ALAssetsLibrary alloc] init] autorelease];
    [library writeVideoAtPathToSavedPhotosAlbum:videourl completionBlock:^(NSURL *assetURL, NSError *error) {

         //書き込み終了後、Unity側へコールバック。
         UnitySendMessage(gameObjectName, callbackMethodName, [error.description UTF8String]);
    }];
}

//extern "C" void _PlaySystemShutterSound ()
//{
    // NOTE:
    //      マナーモードや本体音量に左右されずに鳴る。
   // AudioServicesPlaySystemSound(1108);
//}
