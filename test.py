import cv2
import sys
import json

def find_subimage_position(main_image_path, sub_image_path):
    main_image = cv2.imread(main_image_path)
    sub_image = cv2.imread(sub_image_path)
    result = cv2.matchTemplate(main_image, sub_image, cv2.TM_CCOEFF_NORMED)
    _, _, _, max_loc = cv2.minMaxLoc(result)
    position = {"x": int(max_loc[0]), "y": int(max_loc[1])}
    return position

if __name__ == "__main__":
    main_image_path = sys.argv[1]
    sub_image_path = sys.argv[2]
    position = find_subimage_position(main_image_path, sub_image_path)
    print(json.dumps(position))  # 결과를 JSON 문자열로 출력
