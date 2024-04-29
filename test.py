import cv2
import sys
import json

def find_subimage_position(main_image_path, sub_image_path, threshold=0.8):
    main_image = cv2.imread(main_image_path)
    sub_image = cv2.imread(sub_image_path)
    # 이미지 또는 서브 이미지 로딩 실패 시 None 반환
    if main_image is None or sub_image is None:
        return None
    result = cv2.matchTemplate(main_image, sub_image, cv2.TM_CCOEFF_NORMED)
    min_val, max_val, min_loc, max_loc = cv2.minMaxLoc(result)
    # 최대 매칭 점수가 threshold보다 낮으면 이미지가 없다고 판단
    if max_val < threshold:
        position = {"x": -1, "y": -1}
        return position
    else:
        position = {"x": int(max_loc[0]), "y": int(max_loc[1])}
        return position

if __name__ == "__main__":
    main_image_path = sys.argv[1]
    sub_image_path = sys.argv[2]
    threshold = float(sys.argv[3])
    position = find_subimage_position(main_image_path, sub_image_path, threshold)
    if position is not None:
        print(json.dumps(position))