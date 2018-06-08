using VrmArPlaye;
using Zenject;

namespace VrmArPlayer
{
    public class ForSmartphoneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            //ARモード

            //入力イベントはPhotonの同期された値を使う
            var provider = gameObject.AddComponent<PhotonEventProvider>();
            Container.Bind<IInputEventProvider>().FromInstance(provider);
            Container.Bind<IInputSetable>().FromInstance(provider);

            //ステージサイズを変更できる
            var scaler = gameObject.AddComponent<FlexibleStageScaler>();
            Container.Bind<IStageScaler>().FromInstance(scaler);
            Container.Bind<IStageScaleChanger>().FromInstance(scaler);

            var mainConfig = new Configs(isVrMode: false);
            Container.Bind<Configs>().FromInstance(mainConfig).AsSingle();
        }
    }
}
