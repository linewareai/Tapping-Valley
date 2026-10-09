using System.Collections.Generic;
using UnityEngine;

namespace ValleyTapping
{
    public sealed class SceneNavigator : MonoBehaviour
    {
        [System.Serializable]
        public sealed class PageEntry
        {
            public string pageId;
            public GameObject root;
        }

        [SerializeField] private PageEntry[] pages;
        [SerializeField] private GameObject modalRoot;
        private readonly Stack<string> history = new Stack<string>();
        private string currentPage;

        private void Start()
        {
            if (pages != null)
                foreach (PageEntry page in pages)
                    if (page != null && page.root != null) page.root.SetActive(false);
            if (pages != null && pages.Length > 0 && pages[0] != null)
                ShowPage(pages[0].pageId);
            if (modalRoot != null) modalRoot.SetActive(false);
        }

        public void ShowPage(string pageId)
        {
            PageEntry target = FindPage(pageId);
            if (target == null || target.root == null) return;
            if (!string.IsNullOrEmpty(currentPage) && currentPage != pageId) history.Push(currentPage);
            currentPage = pageId;
            foreach (PageEntry page in pages)
                if (page != null && page.root != null) page.root.SetActive(page.pageId == currentPage);
        }

        public void Back()
        {
            if (modalRoot != null && modalRoot.activeSelf)
            {
                CloseModal();
                return;
            }
            if (history.Count > 0) ShowPageWithoutHistory(history.Pop());
        }

        public void OpenModal()
        {
            if (modalRoot != null) modalRoot.SetActive(true);
        }

        public void CloseModal()
        {
            if (modalRoot != null) modalRoot.SetActive(false);
        }

        private PageEntry FindPage(string id)
        {
            if (pages == null) return null;
            foreach (PageEntry page in pages)
                if (page != null && page.pageId == id) return page;
            return null;
        }

        private void ShowPageWithoutHistory(string id)
        {
            currentPage = id;
            foreach (PageEntry page in pages)
                if (page != null && page.root != null) page.root.SetActive(page.pageId == currentPage);
        }
    }
}
