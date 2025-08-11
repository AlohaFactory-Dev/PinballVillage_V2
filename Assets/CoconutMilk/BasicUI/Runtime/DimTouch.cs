using Aloha.Coconut.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Aloha.CoconutMilk
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class DimTouch : UISlice, IPointerDownHandler
    {
        private IDimClosable _dimClosable;

        public void OnPointerDown(PointerEventData eventData)
        {
            _dimClosable?.CloseByDim();
        }

        protected override void Open(UIOpenArgs openArgs)
        {
            base.Open(openArgs);
            _dimClosable = CurrentView.GetSlice<IDimClosable>();
        }
    }
}