using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebAPI.App_Code;
using WebAPI.Code;

namespace WebAPI.Controllers
{
    public class ImageController : ApiController
    {
        // GET: api/Image
        [ActionName("Get")]
        public HttpResponseMessage Get(string guid)
        {
            var img = TestImage.testLocal(guid);
            //var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            //var imgByte = MergeProvider.Download(Path.Combine(dir, "demo.png"));
            byte[] imgByte;
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                imgByte = ms.ToArray();
            }

            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                //Content = new StreamContent(imgStream)
                ////或者
                Content = new ByteArrayContent(imgByte)
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            return resp;
        }
        // GET: api/Image
        [ActionName("Check")]
        public IHttpActionResult Get(string guid, string select)
        {
            var result = ClientResult.Error("获取数据失败");
            try
            {
                var cache = new CacheHelper();
                var cache_selectIndex = cache.Get<List<int>>("captcha_" + guid);
                var selectIndex = select.Split(',');
                if (cache_selectIndex.Count == selectIndex.Length)
                {
                    foreach (var item in selectIndex)
                    {
                        if (cache_selectIndex.IndexOf(int.Parse(item)) < 0)
                        {
                            throw new Exception("请选择正确的图案");
                        }
                    }
                    result = ClientResult.Success("恭喜,选择正确");
                }
                else
                {
                    result = ClientResult.Error("请选择正确的图案");
                }
            }
            catch (Exception)
            {
                result = ClientResult.Error("请选择正确的图案");
            }
            return Json(result);
        }
    }

    public class TestImage
    {
        /// <summary>
        /// 测试本地图片
        /// </summary>
        public static Image testLocal(string guid)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");

            List<KeyValuePair<string, List<string>>> imageGroup = new List<KeyValuePair<string, List<string>>>();
            var itemImage1 = new KeyValuePair<string, List<string>>("芒果在线", new List<string>() {
                "3.png","4.png"
            });

            var itemImage2 = new KeyValuePair<string, List<string>>("建筑物", new List<string>() {
                "5.jpg","6.jpg", "8.jpg"
            });
            var itemImage3 = new KeyValuePair<string, List<string>>("车辆", new List<string>() {
                "7.jpg", "9.jpg"
            });
            var itemImage4 = new KeyValuePair<string, List<string>>("卡通人物", new List<string>() {
                "1.png", "2.png"
            });
            imageGroup.Add(itemImage1);
            imageGroup.Add(itemImage2);
            imageGroup.Add(itemImage3);
            imageGroup.Add(itemImage4);

            List<string> images = new List<string>();

            //先选出待识别的类目
            var selectItem = imageGroup[random.Next(0, imageGroup.Count)];
            //乱序，然后取两个
            var selectItem2 = RandomSortList(selectItem.Value);
            //记录选中类目的两个图片
            var selectImg = selectItem2.Take(2);
            images.AddRange(selectImg);

            //将其他类型图片加载出来
            var images_all = new List<string>();
            foreach (var item in imageGroup)
            {
                if (item.Key != selectItem.Key)
                {
                    images_all.AddRange(item.Value);
                }
            }
            //乱序，然后去7个
            var images_all2 = RandomSortList(images_all);
            images.AddRange(images_all2.Take(9 - images.Count));
            //防止不满9个处理
            for (int i = 0; i < 9 - images.Count; i++)
            {
                images.Add(images_all[random.Next(0, images_all.Count)]);
            }

            //乱序，生成一张整图
            var imageRandom = RandomSortList(images);

            //标记乱序后图片的位置
            var selectImgIndex = new List<int>();
            foreach (var item in selectImg)
            {
                selectImgIndex.Add(imageRandom.IndexOf(item));
            }

            //图片路径处理
            for (int i = 0; i < imageRandom.Count; i++)
            {
                imageRandom[i] = Path.Combine(dir, imageRandom[i]);
            }

            //计入缓存
            var cache = new CacheHelper();
            cache.Add("captcha_" + guid, selectImgIndex, 10);

            return testMerge("demo", imageRandom, selectItem.Key);
        }

        /// <summary>
        /// 乱序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListT"></param>
        /// <returns></returns>
        public static List<T> RandomSortList<T>(List<T> ListT)
        {
            //Random random = new Random();
            var random = new Random(Guid.NewGuid().GetHashCode());
            List<T> newList = new List<T>();
            foreach (T item in ListT)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            return newList;
        }

        private static Image testMerge(string type, List<string> images, string text)
        {
            var m1 = MergeProvider.Merge9Images(images, text, 344);
            var image1 = MergeProvider.ConvertToImage(m1);
            return image1;
            //var m1Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "/images/merge", type + ".png");
            //if (File.Exists(m1Path))
            //{
            //    File.Delete(m1Path);
            //}
            //image1.Save(m1Path);
        }
    }
}
