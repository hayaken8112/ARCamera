#import <Foundation/Foundation.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVFoundation/AVFoundation.h>


/*
 指定したパスの画像をカメラロールに保存するのに利用するネイティブコード。
 */
// 指定したパスの画像をカメラロールに保存する。
extern "C" void _ImageToAlbum (const char* path)
{
    UIImage *image = [UIImage imageWithContentsOfFile:[NSString stringWithUTF8String:path]];
    ALAssetsLibrary *library = [[[ALAssetsLibrary alloc] init] autorelease];
    NSMutableDictionary *metadata = [[[NSMutableDictionary alloc] init] autorelease];
    [library writeImageToSavedPhotosAlbum:image.CGImage metadata:metadata completionBlock:^(NSURL *assetURL, NSError *error) {
        // 書き込み終了後、Unity側へコールバック。
        //UnitySendMessage({}, {}, [error.description UTF8String]);
    }];
}

//extern "C" void _PlaySystemShutterSound ()
//{
    // NOTE:
    //      マナーモードや本体音量に左右されずに鳴る。
   // AudioServicesPlaySystemSound(1108);
//}
